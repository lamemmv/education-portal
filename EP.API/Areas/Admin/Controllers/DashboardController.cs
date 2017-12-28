using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EP.API.Areas.Admin.Controllers
{
    [Authorize]
    [Route("api/admin/[controller]")]
    public class DashboardController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            //return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
            return new JsonResult(new[]
            {
                new { Id = 1, Name = "Productivity", Url = "productivity" },
                new { Id = 2, Name = "DevOps", Url = "devops" },
                new { Id = 3, Name = "Develioment", Url = "develioment" }
            });
        }
    }
}
