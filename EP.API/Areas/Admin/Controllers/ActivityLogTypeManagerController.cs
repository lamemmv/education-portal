using EP.API.Areas.Admin.ViewModels.Logs;
using EP.Data.Constants;
using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using EP.Services.Logs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EP.API.Areas.Admin.Controllers
{
    public class ActivityLogTypeManagerController : AdminController
    {
        private readonly IActivityLogService _activityLogService;

        public ActivityLogTypeManagerController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        [HttpGet]
        public async Task<IPagedList<ActivityLogType>> Get(int? page, int? size)
        {
            return await _activityLogService.FindLogTypeAsync(page, size);
        }

        [HttpGet("{id}")]
        public async Task<ActivityLogType> Get(string id)
        {
            return await _activityLogService.FindLogTypeAsync(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]ActivityLogTypeViewModel viewModel)
        {
            var oldEntity = await _activityLogService.UpdateLogTypeAsync(id, viewModel.Enabled);

            if (oldEntity == null)
            {
                return NotFound();
            }

            var activityLog = GetUpdatedActivityLog(oldEntity.GetType(), oldEntity.Enabled, viewModel.Enabled);
            await _activityLogService.CreateAsync(SystemKeyword.UpdateActivityLogType, activityLog);

            return NoContent();
        }
    }
}
