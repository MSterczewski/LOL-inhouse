using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDTOs.Account
{
    public class RegisterResponse
    {
        public RegisterResponseDTO? Success { get; set; }
        public ERegisterError? Error { get; set; }
    }
}
