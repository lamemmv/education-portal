using System;

namespace EP.API.Areas.Admin.ViewModels.Logs
{
    public sealed class ActivityLogTypeSearchViewModel
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public string UserName { get; set; }

        public string IP { get; set; }

        public int? Page { get; set; }

        public int? Size { get; set; }
    }
}