using EP.API.ViewModels.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EP.API.Filters
{
    public sealed class ValidateViewModelFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;

            if (!modelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new ApiError(context.ModelState));
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
