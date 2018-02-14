using EP.API.Areas.Admin.ViewModels.Logs;
using EP.API.Areas.Admin.ViewModels;
using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using EP.Services.Logs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EP.API.Areas.Admin.Controllers
{
    public class ActivityLogTypeManagerController : AdminController
    {
        private const string ActivityLogFunctionName = Services.Constants.FunctionName.ActivityLogManagement;

        private readonly IActivityLogService _activityLogService;

        public ActivityLogTypeManagerController(
            IHttpContextAccessor accessor,
            IAuthorizationService authorizationService,
            IActivityLogService activityLogService) : base(accessor, authorizationService)
        {
            _activityLogService = activityLogService;
        }

        [HttpGet]
        public async Task<IPagedList<ActivityLogType>> Get([FromQuery]PaginationSearchViewModel viewModel)
        {
            await AuthorizeReadAsync(ActivityLogFunctionName);

            return await _activityLogService.GetLogTypePagedListAsync(viewModel.Page, viewModel.Size);
        }

        [HttpGet("{id}")]
        public async Task<ActivityLogType> Get(string id)
        {
            await AuthorizeReadAsync(ActivityLogFunctionName);

            return await _activityLogService.GetLogTypeByIdAsync(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]ActivityLogTypeViewModel viewModel)
        {
            await AuthorizeHostAsync(ActivityLogFunctionName);
            
            var result = await _activityLogService.UpdateLogTypeAsync(id, viewModel.Enabled);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
