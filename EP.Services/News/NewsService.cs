using EP.Data.DbContext;
using EP.Data.Entities.Logs;
using EP.Data.Entities.News;
using EP.Data.Paginations;
using EP.Data.Repositories;
using EP.Services.Constants;
using EP.Services.Logs;
using EP.Services.Models;
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
            return await _news.GetPagedListAsync(skip: page, take: size);
        }

        public async Task<NewsItem> GetByIdAsync(string id)
        {
            return await _news.GetByIdAsync(id);
        }

        public async Task<ApiServerResult> CreateAsync(NewsItem entity)
        {
            await _news.CreateAsync(entity);

            //await LogCreateAsync(entity);

            return ApiServerResult.Created(entity.Id);
        }

        public async Task<ApiServerResult> UpdateAsync(NewsItem entity)
        {
            var update = Builders<NewsItem>.Update
                .Set(e => e.Title, entity.Title)
                .Set(e => e.Blob, entity.Blob)
                .Set(e => e.Ingress, entity.Ingress)
                .Set(e => e.Content, entity.Content)
                .Set(e => e.Published, entity.Published)
                .Set(e => e.PublishedDate, entity.PublishedDate)
                .CurrentDate(e => e.UpdatedOn);

            var result = await _news.UpdatePartiallyAsync(entity.Id, update);

            if (!result)
            {
                return ApiServerResult.NotFound();
            }

            //await LogUpdateAsync(entity);

            return ApiServerResult.NoContent();
        }

        public async Task<ApiServerResult> DeleteAsync(string id)
        {
            var projection = Builders<NewsItem>.Projection
                .Exclude(e => e.Blob)
                .Exclude(e => e.Content);
            var oldEntity = await _news.DeleteAsync(id, projection);

            if (oldEntity == null)
            {
                return ApiServerResult.NotFound();
            }

            //await LogDeleteAsync(oldEntity);

            return ApiServerResult.NoContent();
        }

        #region Write Logs

        private async Task LogCreateAsync(NewsItem entity)
        {
            entity.Blob = null;
            entity.Content = null;

            await _activityLogService.CreateAsync(SystemKeyword.CreateNews, entity, null, null);
        }

        private async Task LogUpdateAsync(NewsItem entity)
        {
            entity.Blob = null;
            entity.Content = null;

            await _activityLogService.CreateAsync(SystemKeyword.UpdateNews, entity, null, null);
        }

        private async Task LogDeleteAsync(NewsItem entity)
        {
            await _activityLogService.CreateAsync(SystemKeyword.DeleteNews, entity, null, null);
        }

        #endregion
    }
}
