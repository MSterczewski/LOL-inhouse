using System.Reflection.Metadata;
using InhouseServer.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SessionManager;
using Swashbuckle.AspNetCore.Annotations;
using WebDTOs.Lobby;

namespace InhouseServer.Controllers
{
    [SwaggerResponse(401)]
    public abstract class BaseController : Controller
    {
        protected int? IdFromCookie { get; set; } = null;
        protected virtual bool AdminRequired { get; set; } = false;

        protected bool IsValid()
        {
            var cookie = Request.Headers.Authorization.FirstOrDefault();
            if (cookie == null)
                return false;
            var parts = cookie.Split('_');
            if (parts.Length != 2)
                return false;
            IdFromCookie = int.Parse(parts[0]);

            return Session.IsValid(IdFromCookie.Value, parts[1]);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (
                context.ActionDescriptor
                is not ControllerActionDescriptor controllerActionDescriptor
            )
                return;

            var shouldBypassCookie = controllerActionDescriptor
                .MethodInfo.GetCustomAttributes(inherit: true)
                .Any(a => a.GetType().Equals(typeof(BypassCookieValidation)));
            if (shouldBypassCookie)
                return;
            if (!IsValid())
            {
                context.Result = Unauthorized();
                return;
            }

            if (AdminRequired && !(IdFromCookie.HasValue && Session.IsAdmin(IdFromCookie.Value)))
            {
                context.Result = Forbid();
                return;
            }
        }
    }
}
