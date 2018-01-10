namespace EP.Data.Entities.Logs
{
    public class EmbeddedActivityLogType
    {
        public EmbeddedActivityLogType()
        {
        }

        public EmbeddedActivityLogType(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
