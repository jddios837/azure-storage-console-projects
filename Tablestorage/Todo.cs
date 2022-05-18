using Azure;
using Azure.Data.Tables;

namespace Tablestorage
{
    public class Todo : ITableEntity
    {
        public Todo()
        {
        
        }
    
        public Todo(string name, string description)
        {
            PartitionKey = description;
            RowKey = name;
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public bool Done { get; set; }
    }
}