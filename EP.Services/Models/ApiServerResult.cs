using EP.Services.Enums;

namespace EP.Services.Models
{
    public sealed class ApiServerResult
    {
        private ApiServerResult(ApiStatusCode statusCode, string message = null, object result = null)
        {
            StatusCode = (int)statusCode;
            Message = message;
            Result = result;
        }

        public int StatusCode { get; }

        public string Message { get; }

        public object Result { get; }

        public bool IsOK() => StatusCode == (int)ApiStatusCode.OK;

        public bool IsCreated() => StatusCode == (int)ApiStatusCode.Created;

        public bool IsNoContent() => StatusCode == (int)ApiStatusCode.NoContent;

        public bool IsNotFound() => StatusCode == (int)ApiStatusCode.NotFound;

        public static ApiServerResult OK(object result = null)
        {
            return new ApiServerResult(ApiStatusCode.OK, null, result);
        }

        public static ApiServerResult Created(string id)
        {
            return new ApiServerResult(ApiStatusCode.Created, null, id);
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
