using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services.Matchmaking
{
    public class MatchmakingServiceProgress
    {
        public Dictionary<int, MatchmakingServicePlayer> Players { get; set; }
        public List<int> UnassignedPlayers { get; set; }
        public Dictionary<EPosition, (int player1, int player2)> AssignedPositions { get; set; }
        public List<EPosition> UnassignedPositions { get; set; }
        public Dictionary<EPosition, int> PlayersWaitingInPosition { get; set; }
        public int CurrentThreshold { get; set; }

        public MatchmakingServiceProgress(List<WaitingPlayerDTO> players)
        {
            var playersDict = players.ToDictionary(p => p.Id, p => new MatchmakingServicePlayer(p));
            AssignedPositions = new Dictionary<EPosition, (int player1, int player2)>();
            Players = playersDict;
            UnassignedPlayers = playersDict.Keys.ToList();
            UnassignedPositions = new List<EPosition>
            {
                EPosition.Top,
                EPosition.Jng,
                EPosition.Mid,
                EPosition.Bot,
                EPosition.Supp
            };
            CurrentThreshold = 5;
            PlayersWaitingInPosition = new Dictionary<EPosition, int>();
        }
    }
}
