using Interfaces;
using Models;
using NoSqlRepositoryInterfaces;
using Services.Matchmaking;

namespace Services
{
    public class MatchmakingService(
        IWaitingPlayersRepository waitingPlayersRepository,
        IMatchRepository matchRepository
    ) : IMatchmakingService
    {
        private readonly IWaitingPlayersRepository _waitingPlayersRepository =
            waitingPlayersRepository;
        private readonly IMatchRepository _matchRepository = matchRepository;

        public async Task<MatchDTO> Matchmake()
        {
            var players = await _waitingPlayersRepository.GetAll();
            if (players.Count != 10)
                return new() { UnassignedPlayers = players.Count };

            MatchmakingServiceProgress progress = new(players);

            //MatchmakingServiceAssignmentAboveThreshold.AssignPlayersPairsAboveThresholdLoop(progress);

            MatchmakingServiceAssignmentIfOnlyOnePositionAboveThreshold.AssignIfOnlyOnePositionAboveThreshold(
                progress
            );
            MatchmakingServiceShared.AssignIfOneRoleLeft(progress);

            MatchmakingServiceAssignmentWhenHighDifferenceInPriority.AssignWhenHighDifferenceInPriority(
                progress
            );
            MatchmakingServiceShared.AssignIfOneRoleLeft(progress);

            MatchmakingServiceAssignmentFinal.AssignFinal(progress);

            if (progress.UnassignedPlayers.Count > 0)
                return new MatchDTO() { UnassignedPlayers = progress.UnassignedPlayers.Count };

            var match = MatchmakingServiceTeamsEqualizer.EqualizeTeams(progress);
            await _matchRepository.Set(match);

            return match;
        }
    }
}
