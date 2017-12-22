using EP.API.Areas.Admin.ViewModels.News;
using EP.API.Filters;
using EP.Data.Constants;
using EP.Data.Entities.News;
using EP.Data.Paginations;
using EP.Services.Logs;
using EP.Services.News;
using ExpressMapper.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace EP.API.Areas.Admin.Controllers
{
    public class NewsManagerController : AdminController
    {
        private readonly INewsService _newsService;
        private readonly IActivityLogService _activityLogService;

        public NewsManagerController(
            INewsService newsService,
            IActivityLogService activityLogService)
        {
            _newsService = newsService;
            _activityLogService = activityLogService;
        }

        [HttpGet]
        public async Task<IPagedList<NewsItem>> Get(int? page, int? size)
        {
            return await _newsService.GetPagedListAsync(page, size);
        }

        [HttpGet("{id}")]
        public async Task<NewsItem> Get(string id)
        {
            return await _newsService.GetByIdAsync(id);
        }

        [HttpPost, ValidateViewModel]
        public async Task<IActionResult> Post([FromBody]NewsViewModel viewModel)
        {
            var entity = viewModel.Map<NewsViewModel, NewsItem>();
            entity.CreatedOn = DateTime.UtcNow;

            await _newsService.CreateAsync(entity);

            var activityLog = GetCreatedActivityLog(entity.GetType(), entity);
            await _activityLogService.CreateAsync(SystemKeyword.CreateNews, activityLog);

            return Created(nameof(Post), entity.Id);
        }

        [HttpPut("{id}"), ValidateViewModel]
        public async Task<IActionResult> Put(string id, [FromBody]NewsViewModel viewModel)
        {
            var entity = viewModel.Map<NewsViewModel, NewsItem>();
            entity.Id = id;
            entity.UpdatedOn = DateTime.UtcNow;

            var oldEntity = await _newsService.UpdateAsync(entity);

            if (oldEntity == null)
            {
                return NotFound();
            }

            var activityLog = GetUpdatedActivityLog(oldEntity.GetType(), oldEntity, entity);
            await _activityLogService.CreateAsync(SystemKeyword.UpdateNews, activityLog);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var entity = await _newsService.DeleteAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            var activityLog = GetDeletedActivityLog(entity.GetType(), entity);
            await _activityLogService.CreateAsync(SystemKeyword.DeleteNews, activityLog);

            return NoContent();
        }
    }
}
