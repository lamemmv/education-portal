using EP.Data.DbContext;
using EP.Data.Entities.News;
using EP.Data.Paginations;
using EP.Data.Repositories;
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

        public async Task<IPagedList<NewsItem>> GetPagedListAsync(int? page, int? size)
        {
            return await _news.GetPagedListAsync(skip: page, take: size);
        }

        public async Task<NewsItem> GetByIdAsync(string id)
        {
            return await _news.GetByIdAsync(id);
        }

        public async Task<NewsItem> CreateAsync(NewsItem entity)
        {
            return await _news.CreateAsync(entity);
        }

        public async Task<NewsItem> UpdateAsync(NewsItem entity)
        {
            return await _news.UpdateAsync(entity);
        }

        public async Task<NewsItem> DeleteAsync(string id)
        {
            return await _news.DeleteAsync(id, projection: null);
        }
    }
}
