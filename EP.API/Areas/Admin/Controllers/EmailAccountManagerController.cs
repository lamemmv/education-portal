using EP.API.Areas.Admin.ViewModels.Emails;
using EP.API.Areas.Admin.ViewModels;
using EP.API.Filters;
using EP.Data.Entities.Emails;
using EP.Data.Paginations;
using EP.Services.Emails;
using ExpressMapper.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace EP.API.Areas.Admin.Controllers
{
    public class EmailAccountManagerController : AdminController
    {
        private const string EmailAccountFunctionName = Services.Constants.FunctionName.EmailAccountManagement;

        private readonly IEmailAccountService _emailAccountService;

        public EmailAccountManagerController(
            IHttpContextAccessor accessor,
            IAuthorizationService authorizationService,
            IEmailAccountService emailAccountService) : base(accessor, authorizationService)
        {
            _emailAccountService = emailAccountService;
        }

        [HttpGet]
        public async Task<IPagedList<EmailAccount>> Get([FromQuery]PaginationSearchViewModel viewModel)
        {
            await AuthorizeReadAsync(EmailAccountFunctionName);

            return await _emailAccountService.GetPagedListAsync(viewModel.Page, viewModel.Size);
        }

        [HttpGet("{id}")]
        public async Task<EmailAccount> Get(string id)
        {
            await AuthorizeReadAsync(EmailAccountFunctionName);

            return await _emailAccountService.GetByIdAsync(id);
        }

        [HttpPost, ValidateViewModel]
        public async Task<IActionResult> Post([FromBody]EmailAccountViewModel viewModel)
        {
            await AuthorizeHostAsync(EmailAccountFunctionName);

            var entity = viewModel.Map<EmailAccountViewModel, EmailAccount>();
            await _emailAccountService.CreateAsync(entity, GetEmbeddedUser(), GetClientIP());

            return Created(string.Empty, entity.Id);
        }

        [HttpPut("{id}"), ValidateViewModel]
        public async Task<IActionResult> Put(string id, [FromBody]EmailAccountViewModel viewModel)
        {
            await AuthorizeHostAsync(EmailAccountFunctionName);

            var entity = viewModel.Map<EmailAccountViewModel, EmailAccount>();
            entity.Id = id;

            var result = await _emailAccountService.UpdateAsync(entity, GetEmbeddedUser(), GetClientIP());

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await AuthorizeHostAsync(EmailAccountFunctionName);

            var result = await _emailAccountService.DeleteAsync(id, GetEmbeddedUser(), GetClientIP());

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
