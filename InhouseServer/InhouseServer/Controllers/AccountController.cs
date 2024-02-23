using InhouseServer.Attributes;
using InhouseServer.Hubs;
using Microsoft.AspNetCore.Mvc;
using ServiceInterfaces;
using SignalRInterfaces;
using Swashbuckle.AspNetCore.Annotations;
using WebDTOs.Account;

namespace InhouseServer.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController(IAccountService accountService) : BaseController
    {
        private readonly IAccountService _accountService = accountService;

        [HttpPost]
        [BypassCookieValidation]
        [SwaggerResponse(200, type: typeof(LoginResponseDTO))]
        [SwaggerResponse(400, type: typeof(ELoginError))]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            var response = await _accountService.Login(model);
            if (response.Success == null)
                return BadRequest(response.Error);
            return Json(response.Success);
        }

        [HttpPost]
        [BypassCookieValidation]
        [SwaggerResponse(200, type: typeof(RegisterResponseDTO))]
        [SwaggerResponse(400, type: typeof(ERegisterError))]
        public async Task<IActionResult> Register(RegisterRequestDTO model)
        {
            var response = await _accountService.Register(model);
            if (response.Success == null)
                return BadRequest(response.Error);
            return Json(response.Success);
        }

        [HttpPost]
        [SwaggerResponse(200)]
        public async Task<IActionResult> Logout(LogoutRequestDTO model)
        {
            if (model.Id != IdFromCookie)
                return Unauthorized();
            await _accountService.Logout(model);
            return Ok();
        }

        [HttpPost]
        [SwaggerResponse(200, type: typeof(GetAccountResponseDTO))]
        [SwaggerResponse(400)]
        public async Task<IActionResult> Get(GetAccountRequestDTO model)
        {
            var account = await _accountService.GetAccount(model);
            if (account == null)
                return BadRequest();
            return Ok(account);
        }

        [HttpPost]
        [SwaggerResponse(200)]
        [SwaggerResponse(400, type: typeof(EEditAccountError))]
        public async Task<IActionResult> Edit(EditAccountRequestDTO model)
        {
            var error = await _accountService.EditAccount(model);
            if (error == null)
                return Ok();
            return BadRequest(error.Value);
        }
    }
}
