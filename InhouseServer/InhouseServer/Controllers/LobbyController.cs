using InhouseServer.Attributes;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using SignalRInterfaces;
using Swashbuckle.AspNetCore.Annotations;
using WebDTOs;
using WebDTOs.Lobby;

namespace InhouseServer.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LobbyController(ILobbyDataService lobbyDataService, ILobbyHub lobbyHub)
        : BaseController
    {
        private readonly ILobbyDataService _lobbyDataService = lobbyDataService;
        private readonly ILobbyHub _lobbyHub = lobbyHub;

        [HttpGet]
        [SwaggerResponse(200, type: typeof(GetLobbyResponseDTO))]
        public async Task<IActionResult> Get()
        {
            return Json(await _lobbyDataService.GetLobby());
        }

        [HttpPost]
        [SwaggerResponse(200)]
        [SwaggerResponse(400, type: typeof(EJoinLobbyError))]
        public async Task<IActionResult> Join(JoinLobbyRequestDTO model)
        {
            if (model.Id != IdFromCookie)
                return Unauthorized();
            var error = await _lobbyDataService.JoinLobby(model);
            if (error != null)
                return BadRequest(error);
            await _lobbyHub.RefreshLobby();
            return Ok();
        }

        [HttpPost]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]
        public async Task<IActionResult> Leave(LeaveLobbyRequestDTO model)
        {
            if (model.Id != IdFromCookie)
                return Unauthorized();
            if (!await _lobbyDataService.LeaveLobby(model))
                return BadRequest();
            await _lobbyDataService.LeaveLobby(model);
            await _lobbyHub.RefreshLobby();
            return Ok();
        }
    }
}
