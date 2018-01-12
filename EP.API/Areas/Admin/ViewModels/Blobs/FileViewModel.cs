using Microsoft.AspNetCore.Http;

namespace EP.API.Areas.Admin.ViewModels.Blobs
{
    public sealed class FileViewModel
    {
        public IFormFile[] Files { get; set; }

        public string Parent { get; set; }
    }
}
