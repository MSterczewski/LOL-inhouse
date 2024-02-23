using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services.Matchmaking
{
    /// <summary>
    /// Assignment by checking if given position is the only one above a threshold
    /// This can assign players as "waiting players"
    /// </summary>
    public static class MatchmakingServiceAssignmentIfOnlyOnePositionAboveThreshold
    {
        public static void AssignIfOnlyOnePositionAboveThreshold(
            MatchmakingServiceProgress progress
        )
        {
            progress.CurrentThreshold = 1;
            while (true)
            {
                if (progress.CurrentThreshold == 5 || progress.UnassignedPlayers.Count == 0)
                    break;

                var rolesWithPlayersWithOnePositionAboveThreshold =
                    GetRolesWithPlayersWithJustOnePositionAboveThreshold(progress);

                bool anyChanges = false;
                foreach (var position in rolesWithPlayersWithOnePositionAboveThreshold)
                {
                    int? waitingPlayerId = null;
                    if (
                        progress.PlayersWaitingInPosition.TryGetValue(
                            position.Key,
                            out int waitingPlayer
                        )
                    )
                        waitingPlayerId = waitingPlayer;

                    if (position.Value.Count + (waitingPlayerId.HasValue ? 1 : 0) == 2)
                    {
                        MatchmakingServiceShared.AssignUnsignedAndPotentiallyWaitingPlayersToPosition(
                            progress,
                            position.Key,
                            position.Value[0],
                            waitingPlayerId ?? position.Value[1],
                            waitingPlayerId.HasValue
                        );
                        anyChanges = true;
                    }
                    else if (position.Value.Count == 1)
                    {
                        MatchmakingServiceShared.SetPlayerAsWaiting(
                            progress,
                            position.Key,
                            position.Value[0]
                        );
                        anyChanges = true;
                    }
                }
                if (!anyChanges)
                    progress.CurrentThreshold++;
            }
        }

        private static Dictionary<
            EPosition,
            List<int>
        > GetRolesWithPlayersWithJustOnePositionAboveThreshold(MatchmakingServiceProgress progress)
        {
            var rolesWithPlayersWithOneRoleAboveThreshold =
                progress.UnassignedPositions.ToDictionary(p => p, p => new List<int>());
            foreach (var player in progress.UnassignedPlayers)
            {
                var preferences = progress.Players[player].Preferences;
                var unassignedPositionAboveThreshold = progress
                    .UnassignedPositions.Where(p => preferences[p] > progress.CurrentThreshold)
                    .ToList();

                if (unassignedPositionAboveThreshold.Count == 1)
                    rolesWithPlayersWithOneRoleAboveThreshold[
                        unassignedPositionAboveThreshold.First()
                    ]
                        .Add(player);
            }
            return rolesWithPlayersWithOneRoleAboveThreshold;
        }
    }
}
