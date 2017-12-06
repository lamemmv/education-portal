using System;
using System.Collections.Generic;

namespace EP.Data.Entities.Logs
{
    public class Log : Entity
    {
        public DateTime Timestamp { get; set; }
        
        public string Level { get; set; }

        public string Message { get; set; }

        public string MessageTemplate { get; set; }

        public string Exception { get; set; }

        public IReadOnlyDictionary<string, string> Properties { get; set; }
    }
}
