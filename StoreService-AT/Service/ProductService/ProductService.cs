using StoreService_AT.Model.ValueObjects;
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
        public const string basePath = "v1/Product?";
        public async Task<ProductMessage> FindAllProducts(string name, string categoryId, int page,  int size)
        {
            string newname = "";
            string newcategoryId = "";
            string newpage = "";
            string newsize = "";

            if (name != null)
                newname = "name=" + name + "&";
            if (categoryId != null)
                newcategoryId = "categoryId=" + categoryId + "&";
            if (page != 0)
                newpage = "page=" + page + "&";
            if (size != 0)
                newsize = "size=" + size + "&";
            var response = await _client.GetAsync(basePath + newname + newcategoryId + newpage + newsize);
            return await response.ReadContentAs<ProductMessage>();
        }
        public async Task<ProductMessage> FindAllProducts(int page)
        {
            var response = await _client.GetAsync(basePath+"?page=" + page);
            return await response.ReadContentAs<ProductMessage>();
        }

        public async Task<ProductVo> FindProductById(Guid id)
        {
            var response = await _client.GetAsync($"{basePath}/{id}");
            return await response.ReadContentAs<ProductVo>();
        }
    }
}
