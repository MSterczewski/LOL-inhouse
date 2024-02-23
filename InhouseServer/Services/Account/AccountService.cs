using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DatabaseModels;
using Models;
using NoSqlRepositoryInterfaces;
using RepositoryInterfaces;
using ServiceInterfaces;
using SessionManager;
using WebDTOs;
using WebDTOs.Account;

namespace Services
{
    public partial class AccountService(
        IDbUnitOfWork db,
        IWaitingPlayersRepository waitingPlayersRepository
    ) : IAccountService
    {
        private readonly IDbUnitOfWork _db = db;
        private readonly IWaitingPlayersRepository _waitingPlayersRepository =
            waitingPlayersRepository;

        public async Task Logout(LogoutRequestDTO model)
        {
            if (model.Id == null)
                return;
            await _waitingPlayersRepository.Remove(model.Id.Value);
            Session.Logout(model.Id.Value);
        }

        public async Task<GetAccountResponseDTO?> GetAccount(GetAccountRequestDTO model)
        {
            var player = await _db.Players.GetSingle(p => p.Id == model.Id);
            if (player == null)
                return null;
            return new GetAccountResponseDTO(
                player.Id,
                player.Nickname,
                (ERank)player.Rank,
                (ERole)player.Role,
                player.Top,
                player.Jng,
                player.Mid,
                player.Bot,
                player.Supp
            );
        }

        public async Task<EEditAccountError?> EditAccount(EditAccountRequestDTO model)
        {
            var player = await _db.Players.GetSingle(p => p.Id == model.Id);
            if (player == null)
                return EEditAccountError.PlayerNotFound;
            if (await IsNicknameDuplicate(model.Nickname, player))
                return EEditAccountError.NicknameDuplicate;
            player.Nickname = model.Nickname;
            player.Rank = (int)model.Rank;
            player.Top = model.Top;
            player.Jng = model.Jng;
            player.Mid = model.Mid;
            player.Bot = model.Bot;
            player.Supp = model.Supp;
            await _db.Players.SaveChanges();
            return null;
        }

        private async Task<bool> IsNicknameDuplicate(string nickname, Player player)
        {
            if (player.Nickname != nickname)
            {
                var duplicateNickname = await _db.Players.GetSingle(p => p.Nickname == nickname);
                if (duplicateNickname != null)
                    return true;
            }
            return false;
        }
    }
}
