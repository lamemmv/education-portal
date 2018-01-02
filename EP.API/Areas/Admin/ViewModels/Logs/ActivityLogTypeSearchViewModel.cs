using System;

namespace EP.API.Areas.Admin.ViewModels.Logs
{
    public sealed class ActivityLogTypeSearchViewModel : PaginationSearchViewModel
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public string UserName { get; set; }

        public string IP { get; set; }
    }
}