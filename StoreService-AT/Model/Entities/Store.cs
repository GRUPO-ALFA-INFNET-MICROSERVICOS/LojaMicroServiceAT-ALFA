using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using StoreService_AT.MessageBuss;
using StoreService_AT.Model.VOs;

namespace StoreService_AT.Model.Entities
{
    public class Store : BaseMessage
    {
        [BsonId]
        public Guid Id { get; set; }
        public string StoreName { get; set; }
        public string Telephone { get; set; }
        public Address StoreAdress { get; set; }
        [BsonIgnore]
        public int ProductPage { get; set; }
        [BsonIgnore]
        public int TotalPages { get; set; }
        [BsonIgnore]
        public List<ProductVo> Products { get; set; }
    }
}
