using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;

namespace EP.API.Filters
{
    public sealed class ValidateMimeMultipartContentFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsMultipartContentType(context.HttpContext.Request.ContentType))
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.UnsupportedMediaType);
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType) &&
                contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
