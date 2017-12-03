﻿using EP.Data.Entities;
using EP.Data.Entities.News;

namespace EP.Data.Repositories
{
    public sealed class MongoDbContext : BaseDbContext
    {
        public override void SetupCollections()
        {
        }

        #region News

        private IRepository<NewsItem> _news;

        public override IRepository<NewsItem> News
        {
            get
            {
                _news = _news ?? CreateRepository<NewsItem>("News");

                return _news;
            }
        }

        #endregion

        private IRepository<TEntity> CreateRepository<TEntity>(string collectionName) where TEntity : IEntity
        {
            var collection = MongoDatabase.GetCollection<TEntity>(collectionName);

            return new MongoRepository<TEntity>(collection);
        }
    }
}
