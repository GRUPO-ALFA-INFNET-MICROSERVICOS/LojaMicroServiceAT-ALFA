using StoreService_AT.Model.VOs;

namespace StoreService_AT.Model.ValueObjects
{
    public class ProductMessage
    {
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<ProductVo> Data { get; set; }
    }
}
