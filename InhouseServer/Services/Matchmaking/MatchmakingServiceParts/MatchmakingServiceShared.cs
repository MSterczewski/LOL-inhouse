using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services.Matchmaking
{
    public static class MatchmakingServiceShared
    {
        public static void AssignTwoUnassignedPlayersToPosition(
            MatchmakingServiceProgress progress,
            EPosition position,
            int player1,
            int player2
        )
        {
            progress.AssignedPositions.Add(position, (player1, player2));
            progress.UnassignedPlayers.Remove(player1);
            progress.UnassignedPlayers.Remove(player2);
        }

        public static void AssignUnsignedAndPotentiallyWaitingPlayersToPosition(
            MatchmakingServiceProgress progress,
            EPosition position,
            int player1,
            int player2orWaitingPlayer,
            bool hasWaitingPlayer
        )
        {
            progress.UnassignedPlayers.Remove(player1);
            progress.UnassignedPositions.Remove(position);
            if (hasWaitingPlayer)
            {
                progress.AssignedPositions.Add(position, (player1, player2orWaitingPlayer));
                progress.PlayersWaitingInPosition.Remove(position);
            }
            else
            {
                progress.AssignedPositions.Add(position, (player1, player2orWaitingPlayer));
                progress.UnassignedPlayers.Remove(player2orWaitingPlayer);
            }
        }

        public static void RemoveUnassignedPositionsAssignedInThisIteration(
            MatchmakingServiceProgress progress,
            List<EPosition> positions
        )
        {
            foreach (var position in positions)
                progress.UnassignedPositions.Remove(position);
        }

        public static void AssignIfOneRoleLeft(MatchmakingServiceProgress progress)
        {
            if (progress.UnassignedPositions.Count != 1)
                return;

            var position = progress.UnassignedPositions.First();
            progress.AssignedPositions.Add(
                position,
                (
                    progress.UnassignedPlayers[0],
                    progress.UnassignedPlayers.Count == 2
                        ? progress.UnassignedPlayers[1]
                        : progress.PlayersWaitingInPosition[position]
                )
            );
            progress.UnassignedPlayers.Clear();
            progress.UnassignedPositions.Remove(position);
        }

        public static void SetPlayerAsWaiting(
            MatchmakingServiceProgress progress,
            EPosition position,
            int player
        )
        {
            progress.PlayersWaitingInPosition.Add(position, player);
            progress.UnassignedPlayers.Remove(player);
        }
    }
}
