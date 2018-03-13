using EP.API.Extensions;
using EP.Data.Entities;
using EP.Services.Enums;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

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
            => User.GetEmbeddedUser();

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
