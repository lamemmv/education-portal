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
    public class LogManagerController : AdminController
    {
        private const string LogFunctionName = Services.Constants.FunctionName.LogManagement;

        private readonly ILogService _logService;

        public LogManagerController(
            IHttpContextAccessor accessor,
            IAuthorizationService authorizationService,
            ILogService logService) : base(accessor, authorizationService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<IPagedList<Log>> Get([FromQuery]LogSearchViewModel viewModel)
        {
            await AuthorizeReadAsync(LogFunctionName);

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
            await AuthorizeReadAsync(LogFunctionName);

            return await _logService.GetByIdAsync(id);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string[] ids)
        {
            await AuthorizeHostAsync(LogFunctionName);

            if (ids == null || ids.Length == 0)
            {
                ModelState.AddModelError(nameof(ids), "Ids should not be empty.");

                return BadRequest(ModelState);
            }

            await _logService.DeleteAsync(ids);

            return NoContent();
        }
    }
}