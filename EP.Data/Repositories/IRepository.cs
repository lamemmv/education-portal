using EP.Data.Entities;
using EP.Data.Paginations;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EP.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(
            FilterDefinition<TEntity> filter = null,
            SortDefinition<TEntity> sort = null,
            ProjectionDefinition<TEntity, TEntity> projection = null);

        Task<IPagedList<TEntity>> GetPagedListAsync(
            FilterDefinition<TEntity> filter = null,
            SortDefinition<TEntity> sort = null,
            ProjectionDefinition<TEntity, TEntity> projection = null,
            int? skip = null,
            int? take = null);

        Task<TEntity> GetByIdAsync(
            string id,
            ProjectionDefinition<TEntity, TEntity> projection = null);

        Task<TEntity> GetSingleAsync(
            FilterDefinition<TEntity> filter,
            ProjectionDefinition<TEntity, TEntity> projection = null);

        Task<long> CountAsync(FilterDefinition<TEntity> filter = null);

        Task<TEntity> CreateAsync(TEntity entity);

        Task CreateAsync(IEnumerable<TEntity> entities);

        //Task<bool> UpdateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(
            TEntity entity,
            ProjectionDefinition<TEntity, TEntity> projection = null,
            ReturnDocument returnDocument = ReturnDocument.Before);

        Task<bool> UpdatePartiallyAsync(string id, UpdateDefinition<TEntity> definition);

        Task<TEntity> UpdatePartiallyAsync(
            string id,
            UpdateDefinition<TEntity> definition,
            ProjectionDefinition<TEntity, TEntity> projection = null,
            ReturnDocument returnDocument = ReturnDocument.Before);

        Task<bool> DeleteAsync(string id);

        Task<TEntity> DeleteAsync(
            string id,
            ProjectionDefinition<TEntity, TEntity> projection = null);

        Task<bool> DeleteAsync(FilterDefinition<TEntity> filter = null);

        Task DropCollectionAsync();
    }
}
