using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using EP.Services.Logs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EP.API.Areas.Admin.Controllers
{
    public class ActivityLogController : AdminController
    {
        private readonly IActivityLogService _activityLogService;

        public ActivityLogController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        [HttpGet]
        public async Task<IPagedList<ActivityLog>> Get(
            DateTime createdFrom,
            DateTime createdTo,
            string userName,
            string ip,
            int? page,
            int? size)
        {
            return await _activityLogService.FindAsync(createdFrom, createdTo, userName, ip, page, size);
        }

        [HttpGet("{id}")]
        public async Task<ActivityLog> Get(string id)
        {
            return await _activityLogService.FindAsync(id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(IEnumerable<string> ids)
        {
            var result = await _activityLogService.DeleteAsync(ids);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
