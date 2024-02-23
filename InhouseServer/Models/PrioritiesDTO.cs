using System.Text.Json.Serialization;

namespace Models
{
    public class PrioritiesDTO
    {
        [JsonPropertyName("top")]
        public int Top { get; set; }

        [JsonPropertyName("jng")]
        public int Jng { get; set; }

        [JsonPropertyName("mid")]
        public int Mid { get; set; }

        [JsonPropertyName("bot")]
        public int Bot { get; set; }

        [JsonPropertyName("supp")]
        public int Supp { get; set; }

        public PrioritiesDTO()
        {
            Top = 3;
            Jng = 3;
            Mid = 3;
            Bot = 3;
            Supp = 3;
        }
    }
}
