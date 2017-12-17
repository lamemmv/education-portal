namespace EP.Data.Entities.Blobs
{
    public class Blob : Entity
    {
        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public string ContentType { get; set; }

        public string PhysicalPath { get; set; }
    }
}
