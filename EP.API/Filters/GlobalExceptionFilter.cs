using EP.Services.Enums;
using EP.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;

namespace EP.API.Filters
{
    public sealed class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public GlobalExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            string errorMessage;
            ApiStatusCode statusCode;

            Exception exception = context.Exception;
            Type exceptionType = exception.GetType();

            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                errorMessage = "Unauthorized Access.";
                statusCode = ApiStatusCode.Forbidden;
            }
            else if (exceptionType == typeof(TimeoutException) &&
                exception.Source.StartsWith("MongoDB", StringComparison.OrdinalIgnoreCase))
            {
                errorMessage = "Server is unavailable.";
                statusCode = ApiStatusCode.ServiceUnavailable;
            }
            else
            {
                errorMessage = "An unhandled error occurred.";
                statusCode = ApiStatusCode.InternalServerError;
            }

            HttpContext httpContext = context.HttpContext;
            WriteLog(httpContext, exception);

            HttpResponse response = httpContext.Response;
            response.StatusCode = (int)statusCode;
            response.ContentType = "application/json";

            context.ExceptionHandled = true;
            context.Result = new JsonResult(ApiServerResult.ServerError(statusCode, errorMessage));
        }

        private void WriteLog(HttpContext httpContext, Exception exception)
        {
            try
            {
                HttpRequest request = httpContext.Request;
                ConnectionInfo connection = httpContext.Connection;

                _logger.Error(
                    exception,
                    "Source: {Source}, TraceIdentifier: {TraceIdentifier}, HTTP: {RequestMethod} {RequestPath}?{QueryString}, ConnectionId: {ConnectionId}, RemoteIp: {RemoteIp}, LocalIp: {LocalIp}",
                    exception.Source,
                    httpContext.TraceIdentifier,
                    request.Method,
                    request.Path.Value,
                    request.QueryString.ToString(),
                    connection.Id,
                    connection.RemoteIpAddress.ToString(),
                    connection.LocalIpAddress.ToString());
            }
            catch (Exception)
            {
            }
        }
    }
}
