using EP.Data.DbContext;
using EP.Data.Entities.News;
using EP.Data.Entities;
using EP.Data.Paginations;
using EP.Data.Repositories;
using EP.Services.Constants;
using EP.Services.Logs;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace EP.Services.News
{
    public sealed class NewsService : INewsService
    {
        private readonly IRepository<NewsItem> _news;
        private readonly IActivityLogService _activityLogService;

        public NewsService(
            MongoDbContext dbContext,
            IActivityLogService activityLogService)
        {
            _news = dbContext.News;
            _activityLogService = activityLogService;
        }

        public async Task<IPagedList<NewsItem>> GetPagedListAsync(int? page, int? size)
        {
            var projection = Builders<NewsItem>.Projection
                .Include(e => e.Id)
                .Include(e => e.Title)
                .Include(e => e.Ingress)
                .Include(e => e.Published)
                .Include(e => e.PublishedDate);

            return await _news.GetPagedListAsync(projection: projection, skip: page, take: size);
        }

        public async Task<NewsItem> GetByIdAsync(string id)
            => await _news.GetByIdAsync(id);

        public async Task<NewsItem> CreateAsync(NewsItem entity, EmbeddedUser embeddedUser, string ip)
        {
            await _news.CreateAsync(entity);

            // Activity Log.
            await _activityLogService.CreateAsync(SystemKeyword.CreateNews, entity, embeddedUser, ip);

            return entity;
        }

        public async Task<bool> UpdateAsync(NewsItem entity, EmbeddedUser embeddedUser, string ip)
        {
            var result = await _news.UpdateAsync(entity);

            if (result)
            {
                // Activity Log.
                await _activityLogService.CreateAsync(SystemKeyword.UpdateNews, entity, embeddedUser, ip);
            }

            return result;
        }

        public async Task<bool> DeleteAsync(string id, EmbeddedUser embeddedUser, string ip)
        {
            var projection = Builders<NewsItem>.Projection
                .Exclude(e => e.Blob)
                .Exclude(e => e.Content);
            var oldEntity = await _news.DeleteAsync(id, projection);

            if (oldEntity != null)
            {
                // Activity Log.
                await _activityLogService.CreateAsync(SystemKeyword.DeleteNews, oldEntity, embeddedUser, ip);

                return true;
            }

            return false;
        }
    }
}
