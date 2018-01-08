namespace EP.Data.Entities.Blobs
{
    public class BlobAncestor
    {
        public BlobAncestor()
        {
        }

        public BlobAncestor(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
