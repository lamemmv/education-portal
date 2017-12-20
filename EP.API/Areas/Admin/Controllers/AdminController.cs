using EP.Data.Entities.Logs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace EP.API.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]")]
    public abstract class AdminController : Controller
    {
        protected ActivityLog GetActivityLog(Type objectType, object oldValue = null, object newValue = null)
        {
            var activityLog = new ActivityLog
            {
                CreatedOn = DateTime.UtcNow,
                Creator = null,
                ObjectFullName = objectType.FullName,
                IP = Request.HttpContext.Connection.RemoteIpAddress.ToString()
            };

            if (oldValue != null)
            {
                activityLog.OldValue = ObjectToJson(oldValue);
            }

            if (newValue != null)
            {
                activityLog.NewValue = ObjectToJson(newValue);
            }

            return activityLog;
        }

        protected string ObjectToJson(object value)
        {
            return JsonConvert.SerializeObject(
                value,
                Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}
