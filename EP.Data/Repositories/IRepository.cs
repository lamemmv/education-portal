using EP.Data.Entities;
using EP.Data.Paginations;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EP.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        Task<IEnumerable<TEntity>> FindAsync();

        Task<IPagedList<TEntity>> FindAsync(int page, int size);

        Task<IPagedList<TEntity>> FindAsync(
            FilterDefinition<TEntity> filter,
            SortDefinition<TEntity> sort,
            int page,
            int size);

        Task<TEntity> FindAsync(string id);

        Task<TEntity> CreateAsync(TEntity entity);

        Task<bool> UpdateAsync(string id, TEntity entity);

        Task<TEntity> UpdateAsync(
            string id,
            TEntity entity,
            FindOneAndReplaceOptions<TEntity, TEntity> options = null);

        Task<bool> UpdatePartiallyAsync(string id, UpdateDefinition<TEntity> definition);

        Task<TEntity> UpdatePartiallyAsync(
            string id,
            UpdateDefinition<TEntity> definition,
            FindOneAndUpdateOptions<TEntity, TEntity> options = null);

        Task<bool> DeleteAsync(string id);

        Task<TEntity> DeleteAsync(string id, FindOneAndDeleteOptions<TEntity, TEntity> options = null);

        Task<bool> DeleteAsync();

        Task DropCollectionAsync();
    }
}
