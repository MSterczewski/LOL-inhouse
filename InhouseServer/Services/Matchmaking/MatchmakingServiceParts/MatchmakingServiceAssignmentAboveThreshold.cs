using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services.Matchmaking
{
    /// <summary>
    /// Let's perform a check if there are any positions that have exatly 2 players with high preference to play that role
    /// TODO: let's set "waiting player"
    /// </summary>
    public static class MatchmakingServiceAssignmentAboveThreshold
    {
        public static void AssignPlayersPairsAboveThresholdLoop(MatchmakingServiceProgress progress)
        {
            while (true)
            {
                if (progress.CurrentThreshold == 0)
                    break;
                int assignedInThisIteration = AssignPlayersPairsAboveThreshold(progress);
                if (assignedInThisIteration == 0)
                    progress.CurrentThreshold -= 1;
            }
        }

        private static int AssignPlayersPairsAboveThreshold(MatchmakingServiceProgress progress)
        {
            List<EPosition> assignedPositionsInThisIteration = new List<EPosition>();
            foreach (var position in progress.UnassignedPositions)
            {
                List<int> playersForPosition = new List<int>();

                foreach (var unassignedPlayer in progress.UnassignedPlayers)
                    if (
                        progress.Players[unassignedPlayer].Preferences[position]
                        >= progress.CurrentThreshold
                    )
                        playersForPosition.Add(unassignedPlayer);

                if (playersForPosition.Count == 2)
                {
                    MatchmakingServiceShared.AssignTwoUnassignedPlayersToPosition(
                        progress,
                        position,
                        playersForPosition[0],
                        playersForPosition[1]
                    );
                    assignedPositionsInThisIteration.Add(position);
                }
            }
            MatchmakingServiceShared.RemoveUnassignedPositionsAssignedInThisIteration(
                progress,
                assignedPositionsInThisIteration
            );
            return assignedPositionsInThisIteration.Count;
        }
    }
}
