using EP.Data.Entities;
using EP.Data.Paginations;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EP.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        Task<IEnumerable<TEntity>> FindAsync(
            FilterDefinition<TEntity> filter = null,
            SortDefinition<TEntity> sort = null,
            ProjectionDefinition<TEntity, TEntity> project = null);

        Task<IPagedList<TEntity>> FindAsync(
            FilterDefinition<TEntity> filter = null,
            SortDefinition<TEntity> sort = null,
            ProjectionDefinition<TEntity, TEntity> project = null,
            int? skip = null,
            int? take = null);

        Task<TEntity> FindAsync(string id, FindOptions<TEntity, TEntity> options = null);

        Task<TEntity> FindAsync(
            FilterDefinition<TEntity> filter,
            FindOptions<TEntity, TEntity> options = null);

        Task<long> CountAsync(FilterDefinition<TEntity> filter = null);

        Task<TEntity> CreateAsync(TEntity entity);

        Task CreateAsync(IEnumerable<TEntity> entities);

        Task<bool> UpdateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(
            TEntity entity,
            FindOneAndReplaceOptions<TEntity, TEntity> options);

        Task<bool> UpdatePartiallyAsync(string id, UpdateDefinition<TEntity> definition);

        Task<TEntity> UpdatePartiallyAsync(
            string id,
            UpdateDefinition<TEntity> definition,
            FindOneAndUpdateOptions<TEntity, TEntity> options);

        Task<bool> DeleteAsync(string id);

        Task<TEntity> DeleteAsync(string id, FindOneAndDeleteOptions<TEntity, TEntity> options);

        Task<bool> DeleteAsync(FilterDefinition<TEntity> filter = null);

        Task DropCollectionAsync();
    }
}
