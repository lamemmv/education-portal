using EP.Services.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EP.API.Areas.Admin.Controllers
{
    [Authorize]
    [Route("api/admin/[controller]")]
    public abstract class AdminController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;

        protected AdminController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        protected async Task AuthorizeReadAsync(string functionName)
        {
            await AuthorizeAsync(Permission.Read, functionName);
        }

        protected async Task AuthorizeHostAsync(string functionName)
        {
            await AuthorizeAsync(Permission.Host, functionName);
        }

        protected async Task AuthorizeUploadAsync(string functionName)
        {
            await AuthorizeAsync(Permission.Upload, functionName);
        }

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
