using InhouseServer.Hubs;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using SignalRInterfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace InhouseServer.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MatchmakingController(IMatchmakingService matchmakingService, ILobbyHub lobbyHub)
        : Controller
    {
        private readonly IMatchmakingService _matchmakingService = matchmakingService;
        private readonly ILobbyHub _lobbyHub = lobbyHub;

        [HttpPost]
        [SwaggerResponse(200, type: typeof(MatchDTO))]
        [SwaggerResponse(400)]
        public async Task<IActionResult> Matchmake()
        {
            var match = await _matchmakingService.Matchmake();
            if (match == null || match.UnassignedPlayers > 0)
                return BadRequest();
            await _lobbyHub.MatchReady();
            return Json(match);
        }
    }
}
