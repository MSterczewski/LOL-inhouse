using Models;
using NoSqlRepositoryInterfaces;

namespace NoSqlRepositories
{
    public class WaitingPlayersRepository : IWaitingPlayersRepository
    {
        private static Dictionary<int, WaitingPlayerDTO> WaitingPlayers { get; set; } = [];

        public async Task<bool> Add(WaitingPlayerDTO waitingPlayerDTO)
        {
            if (await HasPlayer(waitingPlayerDTO.Id))
                return false;
            WaitingPlayers.Add(waitingPlayerDTO.Id, waitingPlayerDTO);
            return true;
        }

        public Task Clear()
        {
            WaitingPlayers.Clear();
            return Task.CompletedTask;
        }

        public Task<List<WaitingPlayerDTO>> GetAll()
        {
            return Task.FromResult(WaitingPlayers.Values.ToList());
        }

        public Task<WaitingPlayerDTO?> Get(int id)
        {
            return Task.FromResult(WaitingPlayers.GetValueOrDefault(id));
        }

        public Task<bool> HasPlayer(int id)
        {
            return Task.FromResult(WaitingPlayers.ContainsKey(id));
        }

        public async Task<bool> Remove(int id)
        {
            if (await HasPlayer(id))
            {
                WaitingPlayers.Remove(id);
                return true;
            }
            return false;
        }
    }
}
