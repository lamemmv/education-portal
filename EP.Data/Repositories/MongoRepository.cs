using EP.Data.Entities;
using EP.Data.Paginations;
using MongoDB.Bson;
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

        public async Task<IEnumerable<TEntity>> FindAsync(
            FilterDefinition<TEntity> filter = null,
            SortDefinition<TEntity> sort = null,
            ProjectionDefinition<TEntity, TEntity> project = null)
        {
            filter = filter ?? Builders<TEntity>.Filter.Empty;
            var options = new FindOptions<TEntity, TEntity>
            { 
                Sort = sort ?? Builders<TEntity>.Sort.Descending(e => e.Id)
            };

            if (project != null)
            {
                options.Projection = project;
            }

            var cursor = await _collection.FindAsync(filter, options);

            return await cursor.ToListAsync();
        }

        public async Task<IPagedList<TEntity>> FindAsync(
            FilterDefinition<TEntity> filter = null,
            SortDefinition<TEntity> sort = null,
            ProjectionDefinition<TEntity, TEntity> project = null,
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
                Skip = page <= 1 ? new int?() : (page - 1) * size,
                Limit = size
            };

            if (project != null)
            {
                options.Projection = project;
            }

            var cursor = await _collection.FindAsync(filter, options);

            return PagedList<TEntity>.Create(totalItems, await cursor.ToListAsync(), totalPages, page, size);
        }

        public async Task<TEntity> FindAsync(string id, ProjectionDefinition<TEntity, TEntity> project = null)
        {
            if (string.IsNullOrWhiteSpace(id) || !IsValidObjectId(id))
            {
                return default(TEntity);
            }

            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id.Trim());
            var options = project == null ?
                null :
                new FindOptions<TEntity, TEntity>
                {
                    Projection = project
                };
            var cursor = await _collection.FindAsync(filter, options);

            return await cursor.FirstOrDefaultAsync();
        }

        #endregion

        #region Command

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);

            return entity;
        }

        public async Task<bool> UpdateAsync(string id, TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(id) || !IsValidObjectId(id))
            {
                return false;
            }

            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id.Trim());
            var result = await _collection.ReplaceOneAsync(filter, entity);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<TEntity> UpdateAsync(
            string id,
            TEntity entity,
            FindOneAndReplaceOptions<TEntity, TEntity> options = null)
        {
            if (string.IsNullOrWhiteSpace(id) || !IsValidObjectId(id))
            {
                return default(TEntity);
            }

            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id.Trim());

            return await _collection.FindOneAndReplaceAsync(filter, entity, options);
        }

        public async Task<bool> UpdatePartiallyAsync(string id, UpdateDefinition<TEntity> definition)
        {
            if (string.IsNullOrWhiteSpace(id) || !IsValidObjectId(id))
            {
                return false;
            }

            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id.Trim());
            var result = await _collection.UpdateOneAsync(filter, definition);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<TEntity> UpdatePartiallyAsync(
            string id,
            UpdateDefinition<TEntity> definition,
            FindOneAndUpdateOptions<TEntity, TEntity> options = null)
        {
            if (string.IsNullOrWhiteSpace(id) || !IsValidObjectId(id))
            {
                return default(TEntity);
            }

            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id.Trim());

            return await _collection.FindOneAndUpdateAsync(filter, definition, options);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || !IsValidObjectId(id))
            {
                return false;
            }

            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id.Trim());
            var result = await _collection.DeleteOneAsync(filter);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<TEntity> DeleteAsync(
            string id,
            FindOneAndDeleteOptions<TEntity, TEntity> options = null)
        {
            if (string.IsNullOrWhiteSpace(id) || !IsValidObjectId(id))
            {
                return default(TEntity);
            }

            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id.Trim());

            return await _collection.FindOneAndDeleteAsync(filter, options);
        }

        public async Task<bool> DeleteAsync()
        {
            var document = new BsonDocument();
            var result = await _collection.DeleteManyAsync(document);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task DropCollectionAsync()
        {
            string name = _collection.CollectionNamespace.CollectionName;
            await _collection.Database.DropCollectionAsync(name);
        }

        #endregion

        private static bool IsValidObjectId(string id)
        {
            ObjectId objectId;

            return ObjectId.TryParse(id, out objectId);
        }
    }
}
