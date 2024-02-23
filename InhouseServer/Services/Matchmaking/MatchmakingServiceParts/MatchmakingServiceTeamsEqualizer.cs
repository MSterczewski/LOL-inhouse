using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services.Matchmaking
{
    public static class MatchmakingServiceTeamsEqualizer
    {
        /// <summary>
        /// With given position assignment try to equalize teams
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static MatchDTO EqualizeTeams(MatchmakingServiceProgress progress)
        {
            //Calculate differences in ranks in each position and whether player2 is stronger that player1
            Dictionary<EPosition, int> differenceInRank = new();
            Dictionary<EPosition, bool> player2Stronger = new();
            foreach (var position in progress.AssignedPositions)
            {
                var (player1, player2) = progress.AssignedPositions[position.Key];
                var player1Skill = GetSkillByRank(progress.Players[player1].Rank);
                var player2Skill = GetSkillByRank(progress.Players[player2].Rank);

                player2Stronger.Add(position.Key, player2Skill > player1Skill);

                differenceInRank.Add(position.Key, Math.Abs(player2Skill - player1Skill));
            }

            //Find which positions contain stronger player in team 1
            List<EPosition> team1StrongerPlayer = new();
            int team1SkillSum = 0;
            int team2SkillSum = 0;
            foreach (var position in differenceInRank.OrderByDescending(x => x.Value))
            {
                if (team1SkillSum <= team2SkillSum)
                {
                    team1StrongerPlayer.Add(position.Key);
                    team1SkillSum += position.Value;
                }
                else
                    team2SkillSum += position.Value;
            }

            //now assign players to teams
            Dictionary<EPosition, MatchmakingServicePlayer> team1Players = new();
            Dictionary<EPosition, MatchmakingServicePlayer> team2Players = new();
            foreach (var position in Enum.GetValues(typeof(EPosition)).Cast<EPosition>())
            {
                MatchmakingServicePlayer strongerPlayer;
                MatchmakingServicePlayer weakerPlayer;
                if (player2Stronger.ContainsKey(position))
                {
                    strongerPlayer = progress.Players[progress.AssignedPositions[position].player2];
                    weakerPlayer = progress.Players[progress.AssignedPositions[position].player1];
                }
                else
                {
                    strongerPlayer = progress.Players[progress.AssignedPositions[position].player1];
                    weakerPlayer = progress.Players[progress.AssignedPositions[position].player2];
                }

                if (team1StrongerPlayer.Contains(position))
                {
                    team1Players.Add(position, strongerPlayer);
                    team2Players.Add(position, weakerPlayer);
                }
                else
                {
                    team1Players.Add(position, weakerPlayer);
                    team2Players.Add(position, strongerPlayer);
                }
            }
            //TODO: maybe random
            return new MatchDTO()
            {
                Blue = new MatchDTO.TeamDTO()
                {
                    Players = team1Players.ToDictionary(
                        p => p.Key,
                        p => new MatchDTO.TeamDTO.PlayerDTO(p.Value.Id,p.Value.Rank,p.Value.Nickname)
                    )
                },
                Red = new MatchDTO.TeamDTO()
                {
                    Players = team2Players.ToDictionary(
                        p => p.Key,
                        p => new MatchDTO.TeamDTO.PlayerDTO(p.Value.Id, p.Value.Rank, p.Value.Nickname)
					)
                },
            };
        }

        private static int GetSkillByRank(ERank rank)
        {
            return (int)rank;
        }
    }
}
