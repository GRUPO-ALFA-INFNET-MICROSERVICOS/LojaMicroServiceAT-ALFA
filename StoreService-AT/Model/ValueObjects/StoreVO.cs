using StoreService_AT.Model.Entities;

namespace StoreService_AT.Model.VOs
{
    public class StoreVO
    {
        public Guid Id { get; set; }
        public string StoreName { get; set; }
        public string Telephone { get; set; }
        public Address StoreAdress { get; set; }
    }
}
