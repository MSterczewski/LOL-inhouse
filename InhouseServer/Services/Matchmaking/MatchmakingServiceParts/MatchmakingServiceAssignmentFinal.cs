using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services.Matchmaking
{
    public static class MatchmakingServiceAssignmentFinal
    {
        public static void AssignFinal(MatchmakingServiceProgress progress)
        {
            if (progress.UnassignedPlayers.Count == 0)
                return;
            for (int i = 5; i >= 1; i--)
            {
                Dictionary<EPosition, List<int>> playersWillingToPlayPosition =
                    GetPlayersWillingToPlayPosition(progress, i);

                List<EPosition> nowAssigned = new List<EPosition>();
                var orderedPositions = GetOrderedPositions(progress, playersWillingToPlayPosition);

                foreach (var position in orderedPositions)
                {
                    var willingPlayers = playersWillingToPlayPosition[position];
                    var selectedPlayers = willingPlayers
                        .OrderBy(p =>
                            progress
                                .UnassignedPositions.Except(new List<EPosition>() { position })
                                .Sum(pos => progress.Players[p].Preferences[pos])
                        )
                        .ToList();

                    if (
                        progress.PlayersWaitingInPosition.TryGetValue(
                            position,
                            out int waitingPlayerId
                        )
                    )
                    {
                        if (willingPlayers.Count >= 1)
                        {
                            nowAssigned.Add(position);

                            progress.UnassignedPlayers.Remove(selectedPlayers.First());
                            progress.AssignedPositions.Add(
                                position,
                                (waitingPlayerId, selectedPlayers.First())
                            );
                            progress.PlayersWaitingInPosition.Remove(position);

                            foreach (var position2 in playersWillingToPlayPosition)
                                playersWillingToPlayPosition[position2.Key]
                                    .Remove(selectedPlayers.First());
                        }
                    }
                    else
                    {
                        //if(willingPlayers.Count==1)//TRY WITHOUT THIS?
                        //{
                        //    progress.PlayersWaitingInPosition.Add(position,willingPlayers.First())
                        //}
                        if (willingPlayers.Count >= 2)
                        {
                            nowAssigned.Add(position);

                            progress.UnassignedPlayers.Remove(selectedPlayers[0]);
                            progress.UnassignedPlayers.Remove(selectedPlayers[1]);
                            progress.AssignedPositions.Add(
                                position,
                                (selectedPlayers[0], selectedPlayers[1])
                            );
                            foreach (var position2 in playersWillingToPlayPosition)
                            {
                                playersWillingToPlayPosition[position2.Key]
                                    .Remove(selectedPlayers[0]);
                                playersWillingToPlayPosition[position2.Key]
                                    .Remove(selectedPlayers[1]);
                            }
                        }
                    }
                }
                foreach (var position in nowAssigned)
                    progress.UnassignedPositions.Remove(position);
            }
        }

        private static Dictionary<EPosition, List<int>> GetPlayersWillingToPlayPosition(
            MatchmakingServiceProgress progress,
            int threshold
        )
        {
            Dictionary<EPosition, List<int>> playersWillingToPlayPosition =
                new Dictionary<EPosition, List<int>>();
            foreach (var position in progress.UnassignedPositions)
            {
                playersWillingToPlayPosition.Add(position, new List<int>());
                foreach (var unassignedPlayerId in progress.UnassignedPlayers)
                    if (progress.Players[unassignedPlayerId].Preferences[position] >= threshold)
                        playersWillingToPlayPosition[position].Add(unassignedPlayerId);
            }
            return playersWillingToPlayPosition;
        }

        private static IEnumerable<EPosition> GetOrderedPositions(
            MatchmakingServiceProgress progress,
            Dictionary<EPosition, List<int>> playersWillingToPlayPosition
        )
        {
            return progress
                .UnassignedPositions.Select(p =>
                    (
                        p,
                        progress.PlayersWaitingInPosition.TryGetValue(p, out int _)
                            ? playersWillingToPlayPosition[p].Count + 1
                            : playersWillingToPlayPosition[p].Count
                    )
                )
                .Where(p => p.Item2 >= 2)
                .OrderBy(p => p.Item2)
                .Select(p => p.p);
        }
    }
}
