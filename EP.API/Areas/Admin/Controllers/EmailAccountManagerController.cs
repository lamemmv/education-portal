//using EP.API.Areas.Admin.ViewModels.Emails;
//using EP.API.Areas.Admin.ViewModels;
//using EP.API.Extensions;
//using EP.API.Filters;
//using EP.Data.Entities.Emails;
//using EP.Data.Paginations;
//using EP.Services.Emails;
//using ExpressMapper.Extensions;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using System;

//namespace EP.API.Areas.Admin.Controllers
//{
//    public class EmailAccountManagerController : AdminController
//    {
//        private readonly IEmailAccountService _emailAccountService;

//        public EmailAccountManagerController(IEmailAccountService emailAccountService)
//        {
//            _emailAccountService = emailAccountService;
//        }

//        [HttpGet]
//        public async Task<IPagedList<EmailAccount>> Get([FromQuery]PaginationSearchViewModel viewModel)
//        {
//            return await _emailAccountService.GetPagedListAsync(viewModel.Page, viewModel.Size);
//        }

//        [HttpGet("{id}")]
//        public async Task<EmailAccount> Get(string id)
//        {
//            return await _emailAccountService.GetByIdAsync(id);
//        }

//        [HttpPost, ValidateViewModel]
//        public async Task<IActionResult> Post([FromBody]EmailAccountViewModel viewModel)
//        {
//            var entity = viewModel.Map<EmailAccountViewModel, EmailAccount>();
//            entity.CreatedOn = DateTime.UtcNow;

//            var response = await _emailAccountService.CreateAsync(entity);

//            return response.ToActionResult();
//        }

//        [HttpPut("{id}"), ValidateViewModel]
//        public async Task<IActionResult> Put(string id, [FromBody]EmailAccountViewModel viewModel)
//        {
//            var entity = viewModel.Map<EmailAccountViewModel, EmailAccount>();
//            entity.Id = id;

//            var response = await _emailAccountService.UpdateAsync(entity);

//            return response.ToActionResult();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(string id)
//        {
//            var response = await _emailAccountService.DeleteAsync(id);
            
//            return response.ToActionResult();
//        }
//    }
//}
