using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using SessionManager;
using WebDTOs.Account;

namespace Services
{
    public partial class AccountService
    {
        public async Task<RegisterResponse> Register(RegisterRequestDTO model)
        {
            if (!ValidateRegister(model))
                return ErrorneousRegister(ERegisterError.DataInvalid);
            var existingPlayer = await _db.Players.GetSingle(p => p.Nickname == model.Nickname);
            if (existingPlayer != null)
                return ErrorneousRegister(ERegisterError.NicknameExists);
            var player = new DatabaseModels.Player(
                model.Nickname ?? string.Empty,
                model.Password ?? string.Empty
            );
            await _db.Players.AddAndSave(player);
            return new RegisterResponse()
            {
                Success = new RegisterResponseDTO(
                    model.Nickname ?? string.Empty,
                    player.Id,
                    Session.Login(player.Id, ERole.User),
                    player.ImageUrl
                )
            };
        }

        private static bool ValidateRegister(RegisterRequestDTO model)
        {
            if (
                model == null
                || string.IsNullOrEmpty(model.Password)
                || string.IsNullOrEmpty(model.Nickname)
            )
                return false;
            return true;
        }

        private static RegisterResponse ErrorneousRegister(ERegisterError error)
        {
            return new RegisterResponse() { Error = error };
        }
    }
}
