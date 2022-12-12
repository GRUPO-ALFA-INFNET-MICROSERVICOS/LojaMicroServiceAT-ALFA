using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StoreService_AT.Model.Entities;
using StoreService_AT.Model.ValueObjects;
using StoreService_AT.Model.VOs;
using StoreService_AT.RabbitMQ.Sender;
using StoreService_AT.Repository;
using StoreService_AT.Service.ProductService;
using System.Drawing;
using System.Xml.Linq;

namespace StoreService_AT.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private IStoreRepository _repository;
        private IProductService _productService;

        public StoreController(IStoreRepository repository, IProductService productService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [HttpGet]
        public async Task<List<Store>> GetStores(string name, string categoryId, int page, int size)
        {
            var stores = _repository.GetAllStores().Result;
            try
            {
                if (page == 0)
                    page = 1;
                var allprodutos = await _productService.FindAllProducts(name, categoryId, page, size);
                
                foreach (Store store in stores)
                {
                    store.ProductPage = page;
                    store.TotalPages = allprodutos.TotalPages;
                    store.Products = allprodutos.Data;
                    //_rabbitMQMessageSender.SendMessage(store, "getStoresQueue");
                }
            }
            catch
            {

            }

            return stores;
        }
        [HttpGet("{id}")]
        public async Task<Store> GetById(Guid id, string name, string categoryId, int page, int size)
        {
            var store = _repository.FindStoreById(id).Result;
            var allprodutos = await _productService.FindAllProducts(name, categoryId, page, size);
            store.Products = allprodutos.Data;
            store.TotalPages = allprodutos.TotalPages;
            store.Products = allprodutos.Data;
            return store;
        }
        [HttpPost]
        public async Task<ActionResult<StoreVO>> Create([FromBody] StoreVO storeVo)
        {
            if (storeVo == null) return BadRequest();
            try
            {
                Store store = new()
                {
                    Id = storeVo.Id,
                    StoreName = storeVo.StoreName,
                    Telephone = storeVo.Telephone,
                    StoreAdress = storeVo.StoreAdress,
                };
                store.Id = Guid.NewGuid();
                store.StoreAdress.StoreId = store.Id;
                store.StoreAdress.Id = Guid.NewGuid();
                var newstore = _repository.CreateStore(store).Result;
                //_rabbitMQMessageSender.SendMessage(store, "createStoreQueue");
                return Ok(newstore);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<StoreVO>> Edit([FromBody] StoreVO storeVo, Guid id)
        {
            if (storeVo == null && _repository.FindStoreById(id) == null) return BadRequest();
            try
            {
                Store store = new()
                {
                    Id = storeVo.Id,
                    StoreName = storeVo.StoreName,
                    Telephone = storeVo.Telephone,
                    StoreAdress = storeVo.StoreAdress,
                };
                var newStore = await _repository.UpdateStore(store, id);
                return Ok(newStore);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var status = await _repository.DeleteStore(id);
            if (!status) return BadRequest();
            return Ok("Deletado com sucesso");
        }
    }
}
