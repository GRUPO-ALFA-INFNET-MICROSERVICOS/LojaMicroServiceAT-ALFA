namespace StoreService_AT.Model.VOs
{
    public class StoreVO
    {
        public Guid Id { get; set; }
        public string StoreName { get; set; }
        public string Telephone { get; set; }
        public Adress StoreAdress { get; set; }
        public List<ProductVo> Products { get; set; }
    }
}
