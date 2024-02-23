using Models;

namespace NoSqlRepositoryInterfaces
{
    public interface IWaitingPlayersRepository
    {
        Task<bool> Add(WaitingPlayerDTO waitingPlayerDTO);
        Task Clear();
        Task<List<WaitingPlayerDTO>> GetAll();
        Task<WaitingPlayerDTO?> Get(int id);
        Task<bool> HasPlayer(int id);
        Task<bool> Remove(int id);
    }
}
