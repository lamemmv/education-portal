using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EP.API.Infrastructure
{
    public sealed class FunctionPermissionHandler : AuthorizationHandler<FunctionPermissionRequirement, object>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, FunctionPermissionRequirement requirement, object resource)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                return Task.CompletedTask;
            }

            //var dateOfBirth = Convert.ToDateTime(
            //    context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth).Value);

            //int calculatedAge = DateTime.Today.Year - dateOfBirth.Year;
            
            //if (dateOfBirth > DateTime.Today.AddYears(-calculatedAge))
            //{
            //    calculatedAge--;
            //}

            //if (calculatedAge >= requirement.Age)
            //{
            //    context.Succeed(requirement);
            //}

            return Task.CompletedTask;
        }
    }
}