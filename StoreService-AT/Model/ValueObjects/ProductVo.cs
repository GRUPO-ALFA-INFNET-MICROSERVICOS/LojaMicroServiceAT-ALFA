using StoreService_AT.Model.ValueObjects;

namespace StoreService_AT.Model.VOs
{
    public class ProductVo
    {
        public string id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public CategoryVo category { get; set; }

    }
}
