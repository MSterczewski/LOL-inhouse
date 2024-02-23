using System.Text.Json.Serialization;

namespace WebDTOs.Account
{
    public class RegisterRequestDTO
    {
        [JsonPropertyName("nickname")]
        public string? Nickname { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }
}
