using System;
using System.ComponentModel.DataAnnotations;

namespace EP.API.Areas.Admin.ViewModels.News
{
    public sealed class NewsViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string BlobId { get; set; }

        public string Ingress { get; set; }

        [Required]
        public string Content { get; set; }

        public bool Published { get; set; }
    }
}
