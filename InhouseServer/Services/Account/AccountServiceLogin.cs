using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using SessionManager;
using WebDTOs;
using WebDTOs.Account;

namespace Services
{
    public partial class AccountService
    {
        public async Task<LoginResponse> Login(LoginRequestDTO model)
        {
            if (!ValidateLogin(model))
                return ErrorneousLogin(ELoginError.DataInvalid);
            var player = await _db.Players.GetSingle(p => p.Nickname == model.Nickname);
            if (player == null)
                return ErrorneousLogin(ELoginError.PlayerNotFound);
            if (player.Password != model.Password)
                return ErrorneousLogin(ELoginError.PasswordInvalid);
            return new LoginResponse()
            {
                Success = new LoginResponseDTO(
                    model.Nickname ?? string.Empty,
                    player.Id,
                    Session.Login(player.Id, (ERole)player.Role),
                    (ERole)player.Role,
                    player.ImageUrl
                )
            };
        }

        private static bool ValidateLogin(LoginRequestDTO model)
        {
            if (
                model == null
                || string.IsNullOrEmpty(model.Password)
                || string.IsNullOrEmpty(model.Nickname)
            )
                return false;
            return true;
        }

        private static LoginResponse ErrorneousLogin(ELoginError error)
        {
            return new LoginResponse() { Error = error };
        }
    }
}
