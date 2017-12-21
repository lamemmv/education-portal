using EP.Data.DbContext;
using EP.Data.Entities.News;
using EP.Data.Paginations;
using EP.Data.Repositories;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace EP.Services.News
{
    public sealed class NewsService : INewsService
    {
        private readonly IRepository<NewsItem> _news;

        public NewsService(MongoDbContext dbContext)
        {
            _news = dbContext.News;
        }

        public async Task<IPagedList<NewsItem>> FindAsync(int? page, int? size)
        {
            return await _news.FindAsync(skip: page, take: size);
        }

        public async Task<NewsItem> FindAsync(string id)
        {
            return await _news.FindAsync(id);
        }

        public async Task<NewsItem> CreateAsync(NewsItem entity)
        {
            return await _news.CreateAsync(entity);
        }

        public async Task<NewsItem> UpdateAsync(NewsItem entity)
        {
            var options = new FindOneAndReplaceOptions<NewsItem, NewsItem>
            {
                ReturnDocument = ReturnDocument.Before
            };

            return await _news.UpdateAsync(entity, options);
        }

        public async Task<NewsItem> DeleteAsync(string id)
        {
            return await _news.DeleteAsync(id, options: null);
        }
    }
}
