using EP.Data.Entities;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EP.API.Areas.Admin.Controllers
{
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    [Route("api/admin/[controller]")]
    public abstract class AdminController : ControllerBase
    {
    }
}
