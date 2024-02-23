using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Models;

namespace WebDTOs.Account
{
    public class LoginResponseDTO(
        string nickname,
        int id,
        string token,
        ERole role,
        string imageUrl
    )
    {
        [JsonPropertyName("nickname")]
        public string Nickname { get; set; } = nickname;

        [JsonPropertyName("id")]
        public int Id { get; set; } = id;

        [JsonPropertyName("token")]
        public string Token { get; set; } = token;

        [JsonPropertyName("role")]
        public ERole Role { get; set; } = role;

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; } = imageUrl;
    }
}
