using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Models;

namespace WebDTOs.Lobby
{
    public class GetLobbyResponseDTO(List<WaitingPlayerDTO> players, bool isMatchReady)
    {
        [JsonPropertyName("players")]
        public List<WaitingPlayerDTO> Players { get; set; } = players;

        [JsonPropertyName("isMatchReady")]
        public bool IsMatchReady { get; set; } = isMatchReady;
    }
}
