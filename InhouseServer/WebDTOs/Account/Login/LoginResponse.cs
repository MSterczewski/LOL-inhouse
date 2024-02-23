using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDTOs.Account
{
    public class LoginResponse
    {
        public LoginResponseDTO? Success { get; set; }
        public ELoginError Error { get; set; }
    }
}
