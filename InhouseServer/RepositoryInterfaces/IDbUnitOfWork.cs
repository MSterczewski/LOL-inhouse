using DatabaseModels;

namespace RepositoryInterfaces
{
    public interface IDbUnitOfWork
    {
        IGenericRepository<Player> Players { get; }
    }
}
