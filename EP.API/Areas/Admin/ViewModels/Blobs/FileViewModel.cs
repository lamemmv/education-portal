using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EP.API.Areas.Admin.ViewModels.Blobs
{
    public sealed class FileViewModel
    {
        public IFormFile[] Files { get; set; }

        [Required]
        public string Parent { get; set; }
    }
}
