using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace EP.API.Filters
{
    public sealed class ValidateMimeMultipartContentAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsMultipartContentType(context.HttpContext.Request.ContentType))
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.UnsupportedMediaType);
                return;
            }

            base.OnActionExecuting(context);
        }

        private static bool IsMultipartContentType(string contentType)
            => !string.IsNullOrEmpty(contentType) &&
                contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
    }
}
