using EP.API.Areas.Admin.ViewModels.Emails;
using EP.Data.Entities.Emails;
using EP.Data.Paginations;
using EP.Services.Emails;
using ExpressMapper.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EP.API.Areas.Admin.Controllers
{
    public class EmailAccountManagerController : AdminController
    {
        private readonly IEmailAccountService _emailAccountService;

        public EmailAccountManagerController(IEmailAccountService emailAccountService)
        {
            _emailAccountService = emailAccountService;
        }

        [HttpGet]
        public async Task<IPagedList<EmailAccount>> Get(int? page, int? size)
        {
            return await _emailAccountService.FindAsync(page, size);
        }

        [HttpGet("{id}")]
        public async Task<EmailAccount> Get(string id)
        {
            return await _emailAccountService.FindAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]EmailAccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = viewModel.Map<EmailAccountViewModel, EmailAccount>();
                entity.CreatedOnUtc = DateTime.UtcNow;

                await _emailAccountService.CreateAsync(entity);

                return Created(nameof(Post), entity);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]EmailAccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = viewModel.Map<EmailAccountViewModel, EmailAccount>();

                var result = await _emailAccountService.UpdateAsync(id, entity);

                if (!result)
                {
                    return NotFound();
                }

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _emailAccountService.DeleteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
