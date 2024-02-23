using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebDTOs.Lobby
{
    public class LeaveLobbyRequestDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
