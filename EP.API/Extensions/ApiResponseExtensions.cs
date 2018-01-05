using EP.Services.Enums;
using EP.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace EP.API.Extensions
{
    public static class ApiResponseExtensions
    {
        public static IActionResult ToActionResult(this ApiResponse response)
        {
            ApiStatusCode statusCode = (ApiStatusCode)response.StatusCode;

            switch (statusCode)
            {
                case ApiStatusCode.NotFound:
                    return new NotFoundResult();

                case ApiStatusCode.Created:
                    return new CreatedResult(string.Empty, response.Message);

                case ApiStatusCode.NoContent:
                    return new NoContentResult();

                case ApiStatusCode.BadRequest:
                default:
                    return new BadRequestObjectResult(response);
            }
        }
    }
}
