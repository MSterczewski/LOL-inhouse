using System.Text.Json.Serialization;

namespace WebDTOs.Account
{
    public class LoginRequestDTO
    {
        [JsonPropertyName("nickname")]
        public string? Nickname { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }
}
