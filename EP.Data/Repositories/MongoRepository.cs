using EP.Data.Entities;
using EP.Data.Extensions;
using EP.Data.Paginations;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EP.Data.Repositories
{
    public class MongoRepository<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {
        private const int DefaultPage = 1;
        private const int DefaultSize = 10;

        private readonly IMongoCollection<TEntity> _collection;

        public MongoRepository(IMongoCollection<TEntity> collection)
        {
            _collection = collection;
        }

        #region Query

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            FilterDefinition<TEntity> filter = null,
            SortDefinition<TEntity> sort = null,
            ProjectionDefinition<TEntity, TEntity> projection = null)
        {
            filter = filter ?? Builders<TEntity>.Filter.Empty;
            var options = new FindOptions<TEntity, TEntity>
            {
                Sort = sort,
                Projection = projection
            };

            var cursor = await _collection.FindAsync(filter, options);

            return await cursor.ToListAsync();
        }

        public async Task<IPagedList<TEntity>> GetPagedListAsync(
            FilterDefinition<TEntity> filter = null,
            SortDefinition<TEntity> sort = null,
            ProjectionDefinition<TEntity, TEntity> projection = null,
            int? skip = null,
            int? take = null)
        {
            var page = skip ?? DefaultPage;
            var size = take ?? DefaultSize;

            filter = filter ?? Builders<TEntity>.Filter.Empty;
            var totalItems = await _collection.CountAsync(filter);

            if (totalItems == 0)
            {
                return PagedList<TEntity>.Empty(page, size);
            }

            var totalPages = (int)Math.Ceiling(totalItems / (double)size);

            var options = new FindOptions<TEntity, TEntity>
            {
                Sort = sort ?? Builders<TEntity>.Sort.Descending(e => e.Id),
                Projection = projection,
                Skip = page <= 1 ? new int?() : (page - 1) * size,
                Limit = size
            };

            var cursor = await _collection.FindAsync(filter, options);

            return PagedList<TEntity>.Create(totalItems, await cursor.ToListAsync(), totalPages, page, size);
        }

        public async Task<TEntity> GetByIdAsync(
            string id,
            ProjectionDefinition<TEntity, TEntity> projection = null)
        {
            if (id.IsInvalidObjectId())
            {
                return default(TEntity);
            }

            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);

            return await GetSingleAsync(filter, projection);
        }

        public async Task<TEntity> GetSingleAsync(
            FilterDefinition<TEntity> filter,
            ProjectionDefinition<TEntity, TEntity> projection = null)
        {
            var options = new FindOptions<TEntity, TEntity>
            {
                Projection = projection
            };

            var cursor = await _collection.FindAsync(filter, options);

            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<long> CountAsync(FilterDefinition<TEntity> filter = null)
        {
            filter = filter ?? Builders<TEntity>.Filter.Empty;

            return await _collection.CountAsync(filter);
        }

        #endregion

        #region Command

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);

            return entity;
        }

        public async Task CreateAsync(IEnumerable<TEntity> entities)
            => await _collection.InsertManyAsync(entities);

        //public async Task<bool> UpdateAsync(TEntity entity)
        //{
        //    if (entity.Id.IsInvalidObjectId())
        //    {
        //        return false;
        //    }

        //    var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
        //    var result = await _collection.ReplaceOneAsync(filter, entity);

        //    return result.IsSuccess();
        //}

        public async Task<TEntity> UpdateAsync(
            TEntity entity,
            ProjectionDefinition<TEntity, TEntity> projection = null,
            ReturnDocument returnDocument = ReturnDocument.Before)
        {
            if (entity.Id.IsInvalidObjectId())
            {
                return default(TEntity);
            }

            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            var options = new FindOneAndReplaceOptions<TEntity, TEntity>
            {
                Projection = projection,
                ReturnDocument = returnDocument
            };

            return await _collection.FindOneAndReplaceAsync(filter, entity, options);
        }

        public async Task<bool> UpdatePartiallyAsync(string id, UpdateDefinition<TEntity> definition)
        {
            if (id.IsInvalidObjectId())
            {
                return false;
            }

            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
            var result = await _collection.UpdateOneAsync(filter, definition);

            return result.IsSuccess();
        }

        public async Task<TEntity> UpdatePartiallyAsync(
            string id,
            UpdateDefinition<TEntity> definition,
            ProjectionDefinition<TEntity, TEntity> projection = null,
            ReturnDocument returnDocument = ReturnDocument.Before)
        {
            if (id.IsInvalidObjectId())
            {
                return default(TEntity);
            }

            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
            var options = new FindOneAndUpdateOptions<TEntity, TEntity>
            {
                Projection = projection,
                ReturnDocument = returnDocument
            };

            return await _collection.FindOneAndUpdateAsync(filter, definition, options);
        }

        public async Task<bool> DeleteAsync(string id)
        {
           if (id.IsInvalidObjectId())
           {
               return false;
           }

           var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
           var result = await _collection.DeleteOneAsync(filter);

           return result.IsSuccess();
        }

        public async Task<TEntity> DeleteAsync(
            string id,
            ProjectionDefinition<TEntity, TEntity> projection = null)
        {
            if (id.IsInvalidObjectId())
            {
                return default(TEntity);
            }

            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
            var options = new FindOneAndDeleteOptions<TEntity, TEntity>
            {
                Projection = projection
            };

            return await _collection.FindOneAndDeleteAsync(filter, options);
        }

        public async Task<bool> DeleteAsync(FilterDefinition<TEntity> filter = null)
        {
            filter = filter ?? Builders<TEntity>.Filter.Empty;
            var result = await _collection.DeleteManyAsync(filter);

            return result.IsSuccess();
        }

        public async Task DropCollectionAsync()
        {
            string name = _collection.CollectionNamespace.CollectionName;
            await _collection.Database.DropCollectionAsync(name);
        }

        #endregion
    }
}
