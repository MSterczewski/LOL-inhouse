using Interfaces;
using Models;
using NoSqlRepositoryInterfaces;
using WebDTOs;
using WebDTOs.LobbyAdmin;

namespace Services
{
    public class LobbyAdminDataService(
        IWaitingPlayersRepository waitingPlayersRepository,
        IMatchRepository matchRepository
    ) : ILobbyAdminDataService
    {
        private readonly IWaitingPlayersRepository _waitingPlayersRepository =
            waitingPlayersRepository;
        private readonly IMatchRepository _matchRepository = matchRepository;

        public async Task ResetLobby()
        {
            await _waitingPlayersRepository.Clear();
            await _matchRepository.Clear();
        }

        public async Task<bool> RemovePlayer(RemovePlayerDTO model)
        {
            return await _waitingPlayersRepository.Remove(model.Id);
        }

        public async Task FillWithBots()
        {
            var playersCount = (await _waitingPlayersRepository.GetAll()).Count;

            for (int i = playersCount; i < 10; i++)
            {
                var botId = await GetBotId();
                await _waitingPlayersRepository.Add(
                    new WaitingPlayerDTO()
                    {
                        Nickname = "Bot " + -botId,
                        Id = botId,
                        Priorities = new PrioritiesDTO(),
                        Rank = ERank.Iron4
                    }
                );
            }
        }

        public async Task<int> GetBotId()
        {
            int i = -1;
            for (; i > -20; i--)
                if (!await _waitingPlayersRepository.HasPlayer(i))
                    break;
            return i;
        }

        public async Task ReviewLobby()
        {
            await _matchRepository.Clear();
        }
    }
}
