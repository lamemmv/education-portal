using EP.API.Areas.Admin.ViewModels.Logs;
using EP.API.Extensions;
using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using EP.Services.Extensions;
using EP.Services.Logs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EP.API.Areas.Admin.Controllers
{
    public class ActivityLogManagerController : AdminController
    {
        private readonly IActivityLogService _activityLogService;

        public ActivityLogManagerController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        [HttpGet]
        public async Task<IPagedList<ActivityLog>> Get([FromQuery]ActivityLogTypeSearchViewModel viewModel)
        {
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
            return await _activityLogService.GetByIdAsync(id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _activityLogService.DeleteAsync(id);

            return response.ToActionResult();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(IEnumerable<string> ids)
        {
            var response = await _activityLogService.DeleteAsync(ids);
            
            return response.ToActionResult();
        }
    }
}
