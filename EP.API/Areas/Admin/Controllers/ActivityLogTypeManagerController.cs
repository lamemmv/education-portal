using EP.API.Areas.Admin.ViewModels.Logs;
using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using EP.Services.Logs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EP.API.Areas.Admin.Controllers
{
    public class ActivityLogTypeManagerController : AdminController
    {
        private readonly IActivityLogTypeService _activityLogTypeService;

        public ActivityLogTypeManagerController(IActivityLogTypeService activityLogTypeService)
        {
            _activityLogTypeService = activityLogTypeService;
        }

        [HttpGet]
        public async Task<IPagedList<ActivityLogType>> Get(int? page, int? size)
        {
            return await _activityLogTypeService.FindAsync(page, size);
        }

        [HttpGet("{id}")]
        public async Task<ActivityLogType> Get(string id)
        {
            return await _activityLogTypeService.FindAsync(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]ActivityLogTypeViewModel viewModel)
        {
            var result = await _activityLogTypeService.UpdateAsync(id, viewModel.Enabled);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
