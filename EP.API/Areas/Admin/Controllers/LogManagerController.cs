using EP.API.Areas.Admin.ViewModels.Logs;
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
    public class LogManagerController : AdminController
    {
        private readonly ILogService _logService;

        public LogManagerController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<IPagedList<Log>> Get([FromQuery]LogSearchViewModel viewModel)
        {
            var from = viewModel.From ?? DateTime.Now.AddDays(-1);
            var to = viewModel.To ?? DateTime.Now;

            return await _logService.GetPagedListAsync(
                from.StartOfDayUtc(),
                to.EndOfDayUtc(),
                viewModel.Levels,
                viewModel.Page,
                viewModel.Size);
        }

        [HttpGet("{id}")]
        public async Task<Log> Get(string id)
        {
            return await _logService.GetByIdAsync(id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _logService.DeleteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("DeleteMany/{ids}")]
        public async Task<IActionResult> DeleteMany(IEnumerable<string> ids)
        {
            var result = await _logService.DeleteAsync(ids);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}