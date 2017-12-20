namespace EP.API.Areas.Admin.ViewModels.Blobs
{
    public sealed class BlobSearchViewModel
    {
        public string[] Ext { get; set; }

        public int? Page { get; set; }

        public int? Size { get; set; }
    }
}