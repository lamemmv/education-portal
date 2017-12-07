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

        public async Task<IEnumerable<TEntity>> FindAsync()
        {
            var cursor = await _collection.FindAsync(_ => true);

            return await cursor.ToListAsync();
        }

        public async Task<IPagedList<TEntity>> FindAsync(int page, int size)
        {
            var filter = Builders<TEntity>.Filter.Empty;
            var sort = Builders<TEntity>.Sort.Descending(e => e.Id);

            return await FindAsync(filter, sort, page, size);
        }

        public async Task<IPagedList<TEntity>> FindAsync(
            FilterDefinition<TEntity> filter,
            SortDefinition<TEntity> sort,
            int page,
            int size)
        {
            long totalItems = await _collection.CountAsync(filter);

            if (totalItems == 0)
            {
                return PagedList<TEntity>.Empty(page, size);
            }

            page = page <= 0 ? DefaultPage : page;
            size = size <= 0 ? DefaultSize : size;

            int totalPages = (int)Math.Ceiling(totalItems / (double)size);

            var source = _collection.Find(filter).Sort(sort);
            source = page > 1 ?
                source.Skip((page - 1) * size).Limit(size) :
                source.Limit(size);

            return PagedList<TEntity>.Create(totalItems, await source.ToListAsync(), totalPages, page, size);
        }

        public async Task<TEntity> FindAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || !IsValidObjectId(id))
            {
                return default(TEntity);
            }

            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id.Trim());
            var cursor = await _collection.FindAsync(filter);

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
