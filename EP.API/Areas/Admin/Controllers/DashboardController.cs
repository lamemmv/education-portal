using EP.API.Infrastructure;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace EP.API.Areas.Admin.Controllers
{
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    [Route("api/admin/[controller]")]
    public class DashboardController : ControllerBase
    {
        readonly IAuthorizationService _authorizationService;

        public DashboardController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var isAuthorized =
                await _authorizationService.AuthorizeAsync(User, "Host", ContactOperations.BlobManagerOperationName);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
