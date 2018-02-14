using EP.Services.Enums;

namespace EP.API.Models
{
    public sealed class ApiServerResult
    {
        public ApiServerResult(ApiStatusCode statusCode, string message)
        {
            StatusCode = (int)statusCode;
            Message = message;
        }

        public int StatusCode { get; }

        public string Message { get; }
    }
}