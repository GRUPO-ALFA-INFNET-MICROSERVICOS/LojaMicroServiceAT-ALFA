using StoreService_AT.Model.ValueObjects;

namespace StoreService_AT.Model.VOs
{
    public class ProductVo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public CategoryVo Category { get; set; }

    }
}
