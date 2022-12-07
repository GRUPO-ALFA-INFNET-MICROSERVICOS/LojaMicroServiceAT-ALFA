using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using StoreService_AT.MessageBuss;
using StoreService_AT.Model.VOs;

namespace StoreService_AT.Model
{
    public class Store : BaseMessage
    {
        [BsonId]
        public Guid Id { get; set; }
        public string StoreName { get; set; }
        public string Telephone { get; set; }
        public Adress StoreAdress { get; set; }
        [BsonIgnore]
        public List<ProductVo> Products { get; set; }
    }
}
