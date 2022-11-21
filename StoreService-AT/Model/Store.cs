using MessageBus;
using StoreService_AT.Model.VOs;

namespace StoreService_AT.Model
{
    public class Store : BaseMessage
    {
        public Guid Id { get; set; }
        public string StoreName { get; set; }
        public string Telephone { get; set; }
        public Adress StoreAdress { get; set; }
        public List<ProductVo> Products { get; set; }
    }
}
