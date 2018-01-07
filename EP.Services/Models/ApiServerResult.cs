using EP.Services.Enums;

namespace EP.Services.Models
{
    public sealed class ApiServerResult
    {
        private ApiServerResult(ApiStatusCode statusCode, string message = null)
        {
            StatusCode = (int)statusCode;
            Message = message;
        }

        public int StatusCode { get; }

        public string Message { get; }

        public bool IsCreated() => StatusCode == (int)ApiStatusCode.Created;

        public bool IsNoContent() => StatusCode == (int)ApiStatusCode.NoContent;

        public bool IsNotFound() => StatusCode == (int)ApiStatusCode.NotFound;

        public static ApiServerResult Created(string id)
        {
            return new ApiServerResult(ApiStatusCode.Created, id);
        }

        public static ApiServerResult NoContent()
        {
            return new ApiServerResult(ApiStatusCode.NoContent);
        }

        public static ApiServerResult NotFound()
        {
            return new ApiServerResult(ApiStatusCode.NotFound);
        }

        public static ApiServerResult ServerError(ApiStatusCode statusCode, string message)
        {
            return new ApiServerResult(statusCode, message);
        }
    }
}
