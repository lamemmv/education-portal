using EP.Data.Entities.Logs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace EP.API.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]")]
    public abstract class AdminController : ControllerBase
    {
        protected ActivityLog GetCreatedActivityLog(Type objectType, object newValue)
        {
            return GetUpdatedActivityLog(objectType, null, newValue);
        }

        protected ActivityLog GetDeletedActivityLog(Type objectType, object oldValue)
        {
            return GetUpdatedActivityLog(objectType, oldValue, null);
        }

        protected ActivityLog GetUpdatedActivityLog(Type objectType, object oldValue, object newValue)
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

        private static string ObjectToJson(object value)
        {
            return JsonConvert.SerializeObject(
                value,
                Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}
