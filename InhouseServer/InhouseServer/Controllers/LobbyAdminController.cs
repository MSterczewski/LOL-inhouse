using InhouseServer.Hubs;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using SignalRInterfaces;
using Swashbuckle.AspNetCore.Annotations;
using WebDTOs.LobbyAdmin;

namespace InhouseServer.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LobbyAdminController(
        ILobbyAdminDataService lobbyAdminDataService,
        ILobbyHub lobbyHub
    ) : BaseAdminController
    {
        private readonly ILobbyAdminDataService _lobbyAdminDataService = lobbyAdminDataService;
        private readonly ILobbyHub _lobbyHub = lobbyHub;

        [HttpPost]
        [SwaggerResponse(200)]
        public async Task<IActionResult> Reset()
        {
            await _lobbyAdminDataService.ResetLobby();
            await _lobbyHub.RefreshLobby();
            return Ok();
        }

        [HttpPost]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]
        public async Task<IActionResult> RemovePlayer(RemovePlayerDTO model)
        {
            if (!await _lobbyAdminDataService.RemovePlayer(model))
                return BadRequest();
            await _lobbyHub.RefreshLobby();
            return Ok();
        }

        [HttpPost]
        [SwaggerResponse(200)]
        public async Task<IActionResult> FillWithBots()
        {
            await _lobbyAdminDataService.FillWithBots();
            await _lobbyHub.RefreshLobby();
            return Ok();
        }

        [HttpPost]
        [SwaggerResponse(200)]
        public async Task<IActionResult> Review()
        {
            await _lobbyAdminDataService.ReviewLobby();
            await _lobbyHub.RefreshLobby();
            return Ok();
        }
    }
}
