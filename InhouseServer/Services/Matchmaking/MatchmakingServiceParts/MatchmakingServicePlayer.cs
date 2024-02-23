using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services.Matchmaking
{
    public class MatchmakingServicePlayer
    {
        public int Id { get; set; }
        public ERank Rank { get; set; }
        public string Nickname { get; set; }
        public Dictionary<EPosition, int> Preferences { get; set; }

        public MatchmakingServicePlayer(WaitingPlayerDTO player)
        {
            Id = player.Id;
            Rank = player.Rank;
            Preferences = new Dictionary<EPosition, int>()
            {
                { EPosition.Top, player.Priorities.Top },
                { EPosition.Jng, player.Priorities.Jng },
                { EPosition.Mid, player.Priorities.Mid },
                { EPosition.Bot, player.Priorities.Bot },
                { EPosition.Supp, player.Priorities.Supp },
            };
            Nickname = player.Nickname;
        }
    }
}
