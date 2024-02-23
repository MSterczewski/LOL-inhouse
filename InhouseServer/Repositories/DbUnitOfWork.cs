using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using DatabaseModels;
using RepositoryInterfaces;

namespace Repositories
{
    public class DbUnitOfWork : IDbUnitOfWork, IDisposable
    {
        private readonly MyDbContext context = new();
        private GenericRepository<Player>? playersRepository = null;

        public IGenericRepository<Player> Players
        {
            get
            {
                this.playersRepository ??= new GenericRepository<Player>(context);
                return playersRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
