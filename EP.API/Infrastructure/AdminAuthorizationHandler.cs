using System.Threading.Tasks;
using EP.Data.AspNetIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace EP.API.Infrastructure
{
    public static class ContactOperations
    {
        public static OperationAuthorizationRequirement BlobManagerOperationName =
            new OperationAuthorizationRequirement { Name = Constants.BlobManagerFunctionName };
    }

    public class Constants
    {
        public static readonly string BlobManagerFunctionName = "blobmanager";
    }

    public sealed class AdminAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement, string>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            string resource)
        {
            if (context.User == null)
            {
                return Task.FromResult(0);
            }

            var claim = context.User.FindFirst("blobmanager");

            if (claim != null)
            {
                var index = resource == "Read" ? 0 : 1;

                if (claim.Value[index] == 1)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.FromResult(0);
        }
    }
}