using EP.Services.Enums;
using Microsoft.AspNetCore.Authorization;

namespace EP.API.Infrastructure
{
    public sealed class FunctionPermissionRequirement : IAuthorizationRequirement
    {
        public FunctionPermissionRequirement(string functionName, Permission permission)
        {
            FunctionName = functionName;
            Permission = permission;
        }

        public string FunctionName { get; }

        public Permission Permission { get; }
    }
}