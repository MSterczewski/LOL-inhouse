using System.Text.Json.Serialization;

namespace Models
{
    public class WaitingPlayerDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; }

        [JsonPropertyName("rank")]
        public ERank Rank { get; set; }

        [JsonPropertyName("priorities")]
        public PrioritiesDTO Priorities { get; set; }

        public WaitingPlayerDTO()
        {
            Priorities = new PrioritiesDTO();
            Nickname = "Placeholder";
        }
    }
}
