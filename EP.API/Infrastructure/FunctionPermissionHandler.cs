using EP.Services.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Threading.Tasks;

namespace EP.API.Infrastructure
{
    public sealed class FunctionPermissionHandler : AuthorizationHandler<OperationAuthorizationRequirement, Permission>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Permission resource)
        {
            var permissionClaim = context.User.FindFirst(c => c.Type == requirement.Name);

            if (permissionClaim != null &&
                int.TryParse(permissionClaim.Value, out int permissionValue))
            {
                var permission = (Permission)permissionValue;

                if (permission.HasFlag(resource))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}