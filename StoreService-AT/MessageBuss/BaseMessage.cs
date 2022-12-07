using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace StoreService_AT.MessageBuss
{
    public class BaseMessage
    {
        [JsonIgnore]
        [BsonIgnore]
        public long IdBaseMessage { get; set; }
        [JsonIgnore]
        [BsonIgnore]
        public DateTime MessageCreated { get; set; }
    }
}