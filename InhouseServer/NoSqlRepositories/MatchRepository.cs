using Models;
using NoSqlRepositoryInterfaces;

namespace NoSqlRepositories
{
    public class MatchRepository : IMatchRepository
    {
        private static MatchDTO? Match { get; set; }

        public Task<MatchDTO?> Get()
        {
            return Task.FromResult(Match);
        }

        public Task Set(MatchDTO match)
        {
            Match = match;
            return Task.CompletedTask;
        }

        public Task Clear()
        {
            Match = null;
            return Task.CompletedTask;
        }
    }
}
