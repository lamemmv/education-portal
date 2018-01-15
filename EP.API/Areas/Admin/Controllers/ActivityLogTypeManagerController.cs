//using EP.API.Areas.Admin.ViewModels.Logs;
//using EP.API.Areas.Admin.ViewModels;
//using EP.API.Extensions;
//using EP.Data.Entities.Logs;
//using EP.Data.Paginations;
//using EP.Services.Logs;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;

//namespace EP.API.Areas.Admin.Controllers
//{
//    public class ActivityLogTypeManagerController : AdminController
//    {
//        private readonly IActivityLogService _activityLogService;

//        public ActivityLogTypeManagerController(IActivityLogService activityLogService)
//        {
//            _activityLogService = activityLogService;
//        }

//        [HttpGet]
//        public async Task<IPagedList<ActivityLogType>> Get([FromQuery]PaginationSearchViewModel viewModel)
//        {
//            return await _activityLogService.GetLogTypePagedListAsync(viewModel.Page, viewModel.Size);
//        }

//        [HttpGet("{id}")]
//        public async Task<ActivityLogType> Get(string id)
//        {
//            return await _activityLogService.GetLogTypeByIdAsync(id);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Put(string id, [FromBody]ActivityLogTypeViewModel viewModel)
//        {
//            var response = await _activityLogService.UpdateLogTypeAsync(id, viewModel.Enabled);
            
//            return response.ToActionResult();
//        }
//    }
//}
