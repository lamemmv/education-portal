using EP.Services.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace EP.Services.Models
{
    public sealed class ApiResponse
    {
        private ApiResponse(ModelStateDictionary modelState)
        {
            StatusCode = (int)ApiStatusCode.BadRequest;
            Errors = from kvp in modelState
                     from e in kvp.Value.Errors
                     let k = kvp.Key
                     select new ValidationError(k, e.ErrorMessage);
        }

        private ApiResponse(string source, string message)
        {
            StatusCode = (int)ApiStatusCode.BadRequest;
            Errors = new List<ValidationError>
            {
                new ValidationError(source, message)
            };
        }

        private ApiResponse(ApiStatusCode statusCode, string message = null)
        {
            StatusCode = (int)statusCode;
            Message = message;
        }

        public int StatusCode { get; }

        public string Message { get; }

        public IEnumerable<ValidationError> Errors { get; }

        public static ApiResponse BadRequest(ModelStateDictionary modelState)
        {
            return new ApiResponse(modelState);
        }

        public static ApiResponse BadRequest(string source, string message)
        {
            return new ApiResponse(source, message);
        }

        public static ApiResponse OK()
        {
            return new ApiResponse(ApiStatusCode.OK);
        }

        public static ApiResponse Created(string id)
        {
            return new ApiResponse(ApiStatusCode.Created, id);
        }

        public static ApiResponse NoContent()
        {
            return new ApiResponse(ApiStatusCode.NoContent);
        }

        public static ApiResponse NotFound()
        {
            return new ApiResponse(ApiStatusCode.NotFound);
        }

        public static ApiResponse ServerError(ApiStatusCode statusCode, string message)
        {
            return new ApiResponse(statusCode, message);
        }
    }
}
