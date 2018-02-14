using EP.API.Areas.Admin.ViewModels.Logs;
using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using EP.Services.Extensions;
using EP.Services.Logs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace EP.API.Areas.Admin.Controllers
{
    public class ActivityLogManagerController : AdminController
    {
        private const string ActivityLogFunctionName = Services.Constants.FunctionName.ActivityLogManagement;

        private readonly IActivityLogService _activityLogService;

        public ActivityLogManagerController(
            IHttpContextAccessor accessor,
            IAuthorizationService authorizationService,
            IActivityLogService activityLogService) : base(accessor, authorizationService)
        {
            _activityLogService = activityLogService;
        }

        [HttpGet]
        public async Task<IPagedList<ActivityLog>> Get([FromQuery]ActivityLogTypeSearchViewModel viewModel)
        {
            await AuthorizeReadAsync(ActivityLogFunctionName);

            var from = viewModel.From ?? DateTime.Now.AddDays(-1);
            var to = viewModel.To ?? DateTime.Now;

            return await _activityLogService.GetPagedListAsync(
                from.StartOfDayUtc(),
                to.EndOfDayUtc(),
                viewModel.UserName.TrimNull(),
                viewModel.IP.TrimNull(),
                viewModel.Page,
                viewModel.Size);
        }

        [HttpGet("{id}")]
        public async Task<ActivityLog> Get(string id)
        {
            await AuthorizeReadAsync(ActivityLogFunctionName);

            return await _activityLogService.GetByIdAsync(id);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string[] ids)
        {
            await AuthorizeHostAsync(ActivityLogFunctionName);

            if (ids == null || ids.Length == 0)
            {
                ModelState.AddModelError(nameof(ids), "Ids should not be empty.");

                return BadRequest(ModelState);
            }

            await _activityLogService.DeleteAsync(ids);

            return NoContent();
        }
    }
}
