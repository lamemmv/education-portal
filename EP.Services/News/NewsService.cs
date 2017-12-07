using EP.Data.Entities.News;
using EP.Data.Paginations;
using EP.Data.Repositories;
using EP.Data;
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

        public async Task<IPagedList<NewsItem>> FindAsync(int page, int size)
        {
            return await _news.FindAsync(page, size);
        }

        public async Task<NewsItem> FindAsync(string id)
        {
            return await _news.FindAsync(id);
        }

        public async Task<NewsItem> CreateAsync(NewsItem entity)
        {
            return await _news.CreateAsync(entity);
        }

        public async Task<bool> UpdateAsync(string id, NewsItem entity)
        {
            var update = Builders<NewsItem>.Update
                .Set(e => e.Title, entity.Title)
                .Set(e => e.ShortContent, entity.ShortContent)
                .Set(e => e.FullContent, entity.FullContent)
                .Set(e => e.Published, entity.Published)
                .Set(e => e.PublishedDate, entity.PublishedDate)
                .CurrentDate(s => s.UpdatedOnUtc);

            return await _news.UpdatePartiallyAsync(id, update);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _news.DeleteAsync(id);
        }
    }
}
