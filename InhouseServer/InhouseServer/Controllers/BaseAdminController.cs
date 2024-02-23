using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebDTOs.Lobby;

namespace InhouseServer.Controllers
{
    [SwaggerResponse(403)]
    public class BaseAdminController : BaseController
    {
        protected override bool AdminRequired { get; set; } = true;
    }
}
