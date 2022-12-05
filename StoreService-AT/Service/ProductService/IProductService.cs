using StoreService_AT.Model.VOs;

namespace StoreService_AT.Service.ProductService
{
    public interface IProductService
    {
        Task<List<ProductVo>> FindAllProducts();
        Task<ProductVo> FindProductById(Guid id);
    }
}
