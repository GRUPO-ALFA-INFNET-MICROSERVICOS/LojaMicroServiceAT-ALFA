using StoreService_AT.Model.VOs;
using StoreService_AT.Utils;

namespace StoreService_AT.Service.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _client;

        public ProductService(HttpClient client)
        {
            _client = client;
        }
        public const string basePath = "Product";
        public async Task<List<ProductVo>> FindAllProducts()
        {
            var response = await _client.GetAsync(basePath);
            return await response.ReadContentAs<List<ProductVo>>();
        }

        public async Task<ProductVo> FindProductById(Guid id)
        {
            var response = await _client.GetAsync($"{basePath}/{id}");
            return await response.ReadContentAs<ProductVo>();
        }
    }
}
