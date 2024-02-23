using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DatabaseModels;

namespace RepositoryInterfaces
{
    public interface IGenericRepository<TEntity>
        where TEntity : DatabaseModel
    {
        Task<List<TEntity>> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "",
            int? take = null
        );
        Task<TEntity?> GetSingle(
            Expression<Func<TEntity, bool>>? filter = null,
            string includeProperties = ""
        );

        Task Add(TEntity entity);
        Task AddAndSave(TEntity entity);
        Task SaveChanges();
    }
}
