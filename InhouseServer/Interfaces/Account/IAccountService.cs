using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDTOs.Account;

namespace ServiceInterfaces
{
    public interface IAccountService
    {
        Task<RegisterResponse> Register(RegisterRequestDTO model);
        Task<LoginResponse> Login(LoginRequestDTO model);
        Task Logout(LogoutRequestDTO model);
        Task<GetAccountResponseDTO?> GetAccount(GetAccountRequestDTO model);
        Task<EEditAccountError?> EditAccount(EditAccountRequestDTO model);
    }
}
