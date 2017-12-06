using System;
using System.ComponentModel.DataAnnotations;

namespace EP.API.Areas.Admin.ViewModels.News
{
    public sealed class NewsViewModel
    {
        [Required]
        public string Title { get; set; }

        public string ShortContent { get; set; }

        [Required]
        public string FullContent { get; set; }

        public bool Published { get; set; }

        public DateTime? PublishedDate { get; set; }
    }
}
