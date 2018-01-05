using EP.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EP.API.Filters
{
    public sealed class ValidateViewModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;

            if (!modelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(ApiResponse.BadRequest(context.ModelState));
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
