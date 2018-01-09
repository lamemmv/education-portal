using EP.Services.Enums;

namespace EP.Services.Models
{
    public sealed class ApiServerResult
    {
        public ApiServerResult(
            ApiStatusCode statusCode = ApiStatusCode.OK,
            string id = null,
            string message = null)
        {
            StatusCode = (int)statusCode;
            Id = id;
            Message = message;
        }

        public int StatusCode { get; }

        public string Id { get; }

        public string Message { get; }

        public bool IsCreated()
            => StatusCode == (int)ApiStatusCode.Created;

        public bool IsNoContent()
            => StatusCode == (int)ApiStatusCode.NoContent;

        public bool IsNotFound()
            => StatusCode == (int)ApiStatusCode.NotFound;

        public static ApiServerResult Created(string id)
            => new ApiServerResult(ApiStatusCode.Created, id);

        public static ApiServerResult NoContent()
            => new ApiServerResult(ApiStatusCode.NoContent);

        public static ApiServerResult NotFound()
            => new ApiServerResult(ApiStatusCode.NotFound);


        public static ApiServerResult ServerError(ApiStatusCode statusCode, string message)
            => new ApiServerResult(statusCode, message: message);
    }
}
