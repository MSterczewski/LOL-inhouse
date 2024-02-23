using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class MatchDTO
    {
        [JsonPropertyName("blue")]
        public TeamDTO Blue { get; set; }
		[JsonPropertyName("red")]

		public TeamDTO Red { get; set; }
		[JsonPropertyName("unassignedPlayers")]

		public int UnassignedPlayers { get; set; }

        public MatchDTO()
        {
            Blue = new();
            Red = new();
        }

        public class TeamDTO
		{
			[JsonPropertyName("players")]

			public Dictionary<EPosition, PlayerDTO> Players { get; set; }

            public TeamDTO()
            {
                Players = new();
            }

            public class PlayerDTO
			{
				[JsonPropertyName("id")]
				public int Id { get; set; }
				[JsonPropertyName("rank")]
				public ERank Rank { get; set; }
				[JsonPropertyName("nickname")]
				public string Nickname { get; set; }
                public PlayerDTO(int id, ERank rank, string nickname)
                {
                    Id = id;
                    Rank = rank;
                    Nickname = nickname;
                }
            }
        }
    }
}
