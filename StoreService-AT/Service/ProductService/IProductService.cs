using Microsoft.AspNetCore.Mvc.RazorPages;
using StoreService_AT.Model.ValueObjects;
using StoreService_AT.Model.VOs;

namespace StoreService_AT.Service.ProductService
{
    public interface IProductService
    {
        Task<ProductMessage> FindAllProducts(int page);
        Task<ProductMessage> FindAllProducts(string name, string categoryId, int page, int size);
        Task<ProductVo> FindProductById(Guid id);
    }
}
