using EP.API.Models;
using EP.Services.Enums;
using EP.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
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
            var httpContext = context.HttpContext;
            var exception = context.Exception;

            WriteLog(httpContext, exception);

            var result = GetActionResult(exception, out var apiStatusCode);
            var response = httpContext.Response;
            response.StatusCode = (int)apiStatusCode;
            response.ContentType = "application/json";
            context.ExceptionHandled = true;
            context.Result = result;
        }

        private void WriteLog(HttpContext httpContext, Exception exception)
        {
            try
            {
                HttpRequest request = httpContext.Request;
                ConnectionInfo connection = httpContext.Connection;
                var userName = httpContext.User?.Identity?.Name;

                _logger.Error(
                    exception,
                    "Source: {Source}, TraceIdentifier: {TraceIdentifier}, HTTP: {RequestMethod} {RequestPath}?{QueryString}, ConnectionId: {ConnectionId}, RemoteIp: {RemoteIp}, LocalIp: {LocalIp}, UserName: {UserName}",
                    exception.Source,
                    httpContext.TraceIdentifier,
                    request.Method,
                    request.Path.Value,
                    request.QueryString.ToString(),
                    connection.Id,
                    connection.RemoteIpAddress.ToString(),
                    connection.LocalIpAddress.ToString(),
                    userName);
            }
            catch (Exception)
            {
            }
        }

        private static IActionResult GetActionResult(Exception exception, out ApiStatusCode apiStatusCode)
        {
            Type exceptionType = exception.GetType();

            if (exceptionType == typeof(BadRequestException))
            {
                apiStatusCode = ApiStatusCode.BadRequest;
                var badRequestException = (BadRequestException)exception;

                return new BadRequestObjectResult(
                    new ApiServerResult(badRequestException.StatusCode, badRequestException.Message));
            }

            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                apiStatusCode = ApiStatusCode.Forbidden;

                return new ForbidResult();
            }

            if (exceptionType == typeof(TimeoutException) &&
                exception.Source.StartsWith("MongoDB", StringComparison.OrdinalIgnoreCase))
            {
                apiStatusCode = ApiStatusCode.ServiceUnavailable;

                return new JsonResult(
                    new ApiServerResult(apiStatusCode, "Server is unavailable."));
            }

            apiStatusCode = ApiStatusCode.InternalServerError;

            return new JsonResult(
                new ApiServerResult(apiStatusCode, "An unhandled error occurred."));
        }
    }
}
