using Microsoft.AspNetCore.Mvc;

namespace EP.API.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]")]
    public abstract class AdminController : ControllerBase
    {
    }
}
