using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Models;
using NoSqlRepositoryInterfaces;
using RepositoryInterfaces;
using WebDTOs;
using WebDTOs.Lobby;

namespace Services
{
    public class LobbyDataService(
        IDbUnitOfWork db,
        IWaitingPlayersRepository waitingPlayers,
        IMatchRepository matchRepository
    ) : ILobbyDataService
    {
        private readonly IDbUnitOfWork _db = db;
        private readonly IWaitingPlayersRepository _waitingPlayersRepository = waitingPlayers;
        private readonly IMatchRepository _matchRepository = matchRepository;

        public async Task<GetLobbyResponseDTO> GetLobby()
        {
            var players = await _waitingPlayersRepository.GetAll();
            return new GetLobbyResponseDTO(players, await _matchRepository.Get() != null);
        }

        public async Task<EJoinLobbyError?> JoinLobby(JoinLobbyRequestDTO model)
        {
            var player = await _db.Players.GetSingle(p => p.Id == model.Id);
            if (player == null)
                return EJoinLobbyError.PlayerNotFound;
            if (await _waitingPlayersRepository.HasPlayer(model.Id))
                return EJoinLobbyError.PlayerAlreadyInLobby;
            await _waitingPlayersRepository.Add(
                new WaitingPlayerDTO()
                {
                    Id = player.Id,
                    Nickname = player.Nickname,
                    Rank = (ERank)player.Rank,
                    Priorities = model.Priorities
                }
            );
            return null;
        }

        public Task<bool> LeaveLobby(LeaveLobbyRequestDTO model)
        {
            return _waitingPlayersRepository.Remove(model.Id);
        }
    }
}
