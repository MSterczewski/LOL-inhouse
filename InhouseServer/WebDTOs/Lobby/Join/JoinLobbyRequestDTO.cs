using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Models;

namespace WebDTOs.Lobby
{
    public class JoinLobbyRequestDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("priorities")]
        public PrioritiesDTO Priorities { get; set; }

        public JoinLobbyRequestDTO()
        {
            Priorities = new PrioritiesDTO();
        }
    }
}
