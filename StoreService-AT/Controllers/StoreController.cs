using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreService_AT.Model;
using StoreService_AT.Model.VOs;
using StoreService_AT.RabbitMQ.Sender;
using StoreService_AT.Repository;
using StoreService_AT.Service.ProductService;

namespace StoreService_AT.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private IStoreRepository _repository;
        private IRabbitMQMessageSender _rabbitMQMessageSender;
        private IProductService _productService;

        public StoreController(IStoreRepository repository, IRabbitMQMessageSender rabbitMQMessageSender, IProductService productService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _rabbitMQMessageSender = rabbitMQMessageSender ?? throw new ArgumentNullException(nameof(rabbitMQMessageSender));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [HttpGet]
        public async Task<List<Store>> GetStores()
        {
            var stores = _repository.GetAllStores().Result;
            foreach (Store store in stores)
            {
                store.Products = await _productService.FindAllProducts();
                //_rabbitMQMessageSender.SendMessage(store, "getStoresQueue");
            }
            return stores;
        }
        [HttpGet("{id}")]
        public Store GetById(Guid id)
        {
            var aluno = _repository.FindStoreById(id).Result;
            return aluno;
        }
        [HttpPost]
        public async Task<ActionResult<Store>> Create([FromBody] Store store)
        {
            if (store == null) return BadRequest();
            try
            {
                store.Id = Guid.NewGuid();
                store.StoreAdress.StoreId = store.Id;
                var newstore = _repository.CreateStore(store);
                //_rabbitMQMessageSender.SendMessage(store, "createStoreQueue");
                return Ok(newstore);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Edit([FromBody] Store store, Guid idoldStore)
        {
            if (store == null && _repository.FindStoreById(idoldStore) == null) return BadRequest();
            try
            {
                await _repository.UpdateStore(store, idoldStore);
                return Ok();
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
