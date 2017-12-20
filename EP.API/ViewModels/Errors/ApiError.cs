using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace EP.API.ViewModels.Errors
{
    public sealed class ApiError
    {
        public ApiError(ModelStateDictionary modelState)
        {
            ErrorCode = (int)HttpStatusCode.BadRequest;
            ValidationErrors = from kvp in modelState
                               from e in kvp.Value.Errors
                               let k = kvp.Key
                               select new ValidationError(k, e.ErrorMessage);
        }

        public ApiError(HttpStatusCode httpStatusCode, string errorMessage)
        {
            ErrorCode = (int)httpStatusCode;
            ErrorMessage = errorMessage;
        }

        public int ErrorCode { get; }

        public string ErrorMessage { get; }

        public IEnumerable<ValidationError> ValidationErrors { get; }
    }
}
