using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using NoSqlRepositoryInterfaces;
using Swashbuckle.AspNetCore.Annotations;
using WebDTOs.Lobby;

namespace InhouseServer.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MatchController(IMatchRepository matchRepository) : Controller
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        [HttpGet]
        [SwaggerResponse(200, type: typeof(MatchDTO))]
        [SwaggerResponse(400)]
        public async Task<IActionResult> Get()
        {
            var match = await _matchRepository.Get();
            if (match == null)
                return BadRequest();
            return Json(match);
        }
    }
}
