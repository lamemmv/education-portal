using EP.Services.Enums;
using System;

namespace EP.Services.Models
{
    public sealed class BadRequestException : Exception
    {
        public BadRequestException(ApiStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public ApiStatusCode StatusCode { get; }
    }
}