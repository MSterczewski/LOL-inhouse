using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace WebDTOs.Account
{
    public class EditAccountRequestDTO(
        int id,
        string nickname,
        ERank rank,
        int top,
        int jng,
        int mid,
        int bot,
        int supp
    )
    {
        public int Id { get; set; } = id;
        public string Nickname { get; set; } = nickname;
        public ERank Rank { get; set; } = rank;
        public int Top { get; set; } = top;
        public int Jng { get; set; } = jng;
        public int Mid { get; set; } = mid;
        public int Bot { get; set; } = bot;
        public int Supp { get; set; } = supp;
    }
}
