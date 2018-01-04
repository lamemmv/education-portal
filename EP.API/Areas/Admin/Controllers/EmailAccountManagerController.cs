using EP.API.Areas.Admin.ViewModels.Emails;
using EP.API.Areas.Admin.ViewModels;
using EP.API.Filters;
using EP.Data.Entities.Emails;
using EP.Data.Paginations;
using EP.Services.Constants;
using EP.Services.Emails;
using EP.Services.Logs;
using ExpressMapper.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace EP.API.Areas.Admin.Controllers
{
    public class EmailAccountManagerController : AdminController
    {
        private readonly IEmailAccountService _emailAccountService;
        private readonly IActivityLogService _activityLogService;

        public EmailAccountManagerController(
            IEmailAccountService emailAccountService,
            IActivityLogService activityLogService)
        {
            _emailAccountService = emailAccountService;
            _activityLogService = activityLogService;
        }

        [HttpGet]
        public async Task<IPagedList<EmailAccount>> Get([FromQuery]PaginationSearchViewModel viewModel)
        {
            return await _emailAccountService.GetPagedListAsync(viewModel.Page, viewModel.Size);
        }

        [HttpGet("{id}")]
        public async Task<EmailAccount> Get(string id)
        {
            return await _emailAccountService.GetByIdAsync(id);
        }

        [HttpPost, ValidateViewModel]
        public async Task<IActionResult> Post([FromBody]EmailAccountViewModel viewModel)
        {
            var entity = viewModel.Map<EmailAccountViewModel, EmailAccount>();
            entity.CreatedOn = DateTime.UtcNow;

            await _emailAccountService.CreateAsync(entity);

            // Remove password when writing log.
            entity.Password = null;
            var activityLog = GetCreatedActivityLog(entity.GetType(), entity);
            await _activityLogService.CreateAsync(SystemKeyword.CreateEmailAccount, activityLog);

            return Created(nameof(Post), entity);
        }

        [HttpPut("{id}"), ValidateViewModel]
        public async Task<IActionResult> Put(string id, [FromBody]EmailAccountViewModel viewModel)
        {
            var entity = viewModel.Map<EmailAccountViewModel, EmailAccount>();
            entity.Id = id;
            entity.UpdatedOn = DateTime.UtcNow;

            var oldEntity = await _emailAccountService.UpdateAsync(entity);

            if (oldEntity == null)
            {
                return NotFound();
            }

            var activityLog = GetUpdatedActivityLog(oldEntity.GetType(), oldEntity, entity);
            await _activityLogService.CreateAsync(SystemKeyword.UpdateEmailAccount, activityLog);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var entity = await _emailAccountService.DeleteAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            var activityLog = GetDeletedActivityLog(entity.GetType(), entity);
            await _activityLogService.CreateAsync(SystemKeyword.DeleteEmailAccount, activityLog);

            return NoContent();
        }
    }
}
