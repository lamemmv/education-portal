using System;

namespace EP.API.Areas.Admin.ViewModels.Logs
{
    public sealed class LogSearchViewModel
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public string[] Levels { get; set; }

        public int? Page { get; set; }

        public int? Size { get; set; }
    }
}