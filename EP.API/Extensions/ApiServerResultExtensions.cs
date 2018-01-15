using EP.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace EP.API.Extensions
{
    public static class ApiServerResultExtensions
    {
        public static IActionResult ToActionResult(this ApiServerResult response)
        {
            if (response.IsCreated())
            {
                return new CreatedResult(string.Empty, response.Id);
            }

            if (response.IsNoContent())
            {
                return new NoContentResult();
            }

            if (response.IsNotFound())
            {
                return new NotFoundResult();
            }

            return new BadRequestObjectResult(response);
        }
    }
}
