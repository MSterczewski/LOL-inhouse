using Models;

namespace NoSqlRepositoryInterfaces
{
    public interface IMatchRepository
    {
        Task<MatchDTO?> Get();
        Task Set(MatchDTO match);
        Task Clear();
    }
}
