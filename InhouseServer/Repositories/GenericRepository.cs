using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Database;
using DatabaseModels;
using Microsoft.EntityFrameworkCore;
using RepositoryInterfaces;

namespace Repositories
{
    public class GenericRepository<TEntity>(MyDbContext context) : IGenericRepository<TEntity>
        where TEntity : DatabaseModel
    {
        internal MyDbContext context = context;
        internal DbSet<TEntity> dbSet = context.Set<TEntity>();
        private static readonly char[] separator = [','];

        public Task<List<TEntity>> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "",
            int? take = null
        )
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            foreach (
                var includeProperty in includeProperties.Split(
                    separator,
                    StringSplitOptions.RemoveEmptyEntries
                )
            )
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
                query = orderBy(query);

            if (take != null)
                query = query.Take(take.Value);

            return query.ToListAsync();
        }

        public async Task<TEntity?> GetSingle(
            Expression<Func<TEntity, bool>>? filter = null,
            string includeProperties = ""
        )
        {
            return (await Get(filter, includeProperties: includeProperties))?.FirstOrDefault();
        }

        public async Task Add(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task AddAndSave(TEntity entity)
        {
            await Add(entity);
            await SaveChanges();
        }

        public Task SaveChanges()
        {
            return context.SaveChangesAsync();
        }

        //public ValueTask<TEntity?> GetByID(object id)
        //{
        //    return dbSet.FindAsync(id);
        //}

        //public async Task Delete(object id)
        //{
        //    TEntity? entityToDelete = await GetByID(id);
        //    if (entityToDelete == null)
        //        return;
        //    Delete(entityToDelete);
        //}

        //public virtual void Delete(TEntity entityToDelete)
        //{
        //    if (context.Entry(entityToDelete).State == EntityState.Detached)
        //    {
        //        dbSet.Attach(entityToDelete);
        //    }
        //    dbSet.Remove(entityToDelete);
        //}

        //public virtual void Update(TEntity entityToUpdate)
        //{
        //    dbSet.Attach(entityToUpdate);
        //    context.Entry(entityToUpdate).State = EntityState.Modified;
        //}
    }
}
