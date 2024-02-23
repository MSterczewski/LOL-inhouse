using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services.Matchmaking
{
    /// <summary>
    /// TODO: Consider adding a "anyChanges" like in MatchmakingServiceAssignmentIfOnlyOnePositionAboveThreshold
    /// </summary>
    public static class MatchmakingServiceAssignmentWhenHighDifferenceInPriority
    {
        public static void AssignWhenHighDifferenceInPriority(MatchmakingServiceProgress progress)
        {
            if (progress.UnassignedPlayers.Count == 0)
                return;
            for (int minDiff = 4; minDiff >= 1; minDiff--)
            {
                if (progress.UnassignedPositions.Count == 1)
                    break;
                Dictionary<EPosition, List<int>> highDifferencePlayersByPosition =
                    progress.UnassignedPositions.ToDictionary(p => p, p => new List<int>());
                foreach (var player in progress.UnassignedPlayers)
                {
                    var orderedPreferences = progress
                        .Players[player]
                        .Preferences.Where(p => progress.UnassignedPositions.Contains(p.Key))
                        .OrderByDescending(p => p.Value)
                        .ToList();

                    if (orderedPreferences[0].Value - orderedPreferences[1].Value >= minDiff)
                        highDifferencePlayersByPosition[orderedPreferences[0].Key].Add(player);
                }

                foreach (var position in highDifferencePlayersByPosition)
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
                        MatchmakingServiceShared.AssignUnsignedAndPotentiallyWaitingPlayersToPosition(
                            progress,
                            position.Key,
                            position.Value[0],
                            waitingPlayerId ?? position.Value[1],
                            waitingPlayerId.HasValue
                        );
                    else if (position.Value.Count == 1)
                        MatchmakingServiceShared.SetPlayerAsWaiting(
                            progress,
                            position.Key,
                            position.Value[0]
                        );
                }
            }
        }
    }
}
