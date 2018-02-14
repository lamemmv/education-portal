using EP.Data.Entities;
using EP.Services.Enums;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;
using IdentityModel;

namespace EP.API.Areas.Admin.Controllers
{
    [Authorize]
    [Route("api/admin/[controller]")]
    public abstract class AdminController : ControllerBase
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IAuthorizationService _authorizationService;

        protected AdminController(
            IHttpContextAccessor accessor,
            IAuthorizationService authorizationService)
        {
            _accessor = accessor;
            _authorizationService = authorizationService;
        }

        protected EmbeddedUser GetEmbeddedUser()
        {
            var claims = User?.Claims;

            if (claims?.Any() == true)
            {
                var embeddedUser = new EmbeddedUser();

                var claim = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject);
                embeddedUser.Id = claim.Value;

                claim = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name);
                embeddedUser.UserName = claim.Value;

                claim = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.ClientId);
                embeddedUser.ClientId = claim.Value;

                return embeddedUser;
            }

            return null;
        }

        protected string GetClientIP()
            => _accessor.HttpContext.Connection.RemoteIpAddress.ToString();

        protected async Task AuthorizeReadAsync(string functionName)
            => await AuthorizeAsync(Permission.Read, functionName);

        protected async Task AuthorizeHostAsync(string functionName)
            => await AuthorizeAsync(Permission.Host, functionName);

        protected async Task AuthorizeUploadAsync(string functionName)
            => await AuthorizeAsync(Permission.Upload, functionName);

        private async Task AuthorizeAsync(Permission permission, string functionName)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(
                   User,
                   permission,
                   new OperationAuthorizationRequirement { Name = functionName });

            if (!authorizationResult.Succeeded)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
