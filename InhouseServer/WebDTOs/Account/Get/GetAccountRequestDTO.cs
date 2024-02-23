using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebDTOs.Account
{
    public class GetAccountRequestDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
