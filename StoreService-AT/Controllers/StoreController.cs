using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StoreService_AT.Model.Entities;
using StoreService_AT.Model.ValueObjects;
using StoreService_AT.Model.VOs;
using StoreService_AT.RabbitMQ.Sender;
using StoreService_AT.Repository;
using StoreService_AT.Service.ProductService;
using StoreService_AT.Service.StoreService;
using System.Drawing;
using System.Xml.Linq;

namespace StoreService_AT.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private IStoreService _storeService;
        private IProductService _productService;

        public StoreController(IStoreService storeService, IProductService productService)
        {
            _storeService = storeService ?? throw new ArgumentNullException(nameof(storeService));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [HttpGet]
        public async Task<ActionResult<List<Store>>> GetStores(string storeName,string productName, string ProductCategoryId, int page, int pageSize)
        {
            
            try
            {
                storeName ??= "";
                var stores = _storeService.GetAllStores(storeName).Result;
                if(stores.Count == 0)
                {
                    if(storeName != "") storeName = "Whith The Name: " + storeName;
                    return Ok("None Stores Registered " + storeName);
                }

                if (page == 0) page = 1;
                var allprodutos = await _productService.FindAllProducts(productName, ProductCategoryId, page, pageSize);
                
                foreach (Store store in stores)
                {
                    store.ProductPage = page;
                    store.TotalPages = allprodutos.TotalPages;
                    store.Products = allprodutos.Data;
                    //_rabbitMQMessageSender.SendMessage(store, "getStoresQueue");
                }
                return Ok(stores);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Store>> GetById(Guid id, string productName, string ProductCategoryId, int page, int pageSize)
        {
            var store = _storeService.FindStoreById(id).Result;
            if (store == null) return BadRequest("Store not Found");
            try
            {
                var allprodutos = await _productService.FindAllProducts(productName, ProductCategoryId, page, pageSize);
                store.Products = allprodutos.Data;
                store.TotalPages = allprodutos.TotalPages;
                store.Products = allprodutos.Data;
                return Ok(store);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
        [HttpPost]
        public async Task<ActionResult<StoreVo>> Create([FromBody] StoreVo storeVo)
        {
            if (storeVo == null) return BadRequest();
            try
            {
                Store store = new()
                {
                    Id = Guid.NewGuid(),
                    StoreName = storeVo.StoreName,
                    Telephone = storeVo.Telephone,
                    StoreAdress = storeVo.StoreAdress,
                };
                store.StoreAdress.StoreId = store.Id;
                store.StoreAdress.Id = Guid.NewGuid();
                var newstore = _storeService.CreateStore(store).Result;
                //_rabbitMQMessageSender.SendMessage(store, "createStoreQueue");
                return Ok(newstore);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut("{id}")]
        public async Task<ActionResult<StoreVo>> Edit([FromBody] StoreVo storeVo, Guid id)
        {
            var storefind = _storeService.FindStoreById(id).Result;
            if(storefind == null) return BadRequest("Store not Found");
            if (storeVo == null) return BadRequest();
            try
            {
                Store store = new()
                {
                    Id = storefind.Id,
                    StoreName = storeVo.StoreName,
                    Telephone = storeVo.Telephone,
                    StoreAdress = storeVo.StoreAdress,
                };
                var newStore = await _storeService.UpdateStore(store, id);
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
            try
            {
                var store = _storeService.FindStoreById(id);
                if(store == null) return BadRequest("Store Not Found");
                var status = await _storeService.DeleteStore(id);
                return Ok("Deleted with success");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
