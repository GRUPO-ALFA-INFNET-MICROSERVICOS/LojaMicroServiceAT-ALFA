using StoreService_AT.Model.Entities;
using StoreService_AT.Repository;

namespace StoreService_AT.Service.StoreService
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;

        public StoreService(IStoreRepository storeService)
        {
            _storeRepository = storeService;
        }
        public async Task<List<Store>> GetAllStores(string name)
        {
            var stores = await _storeRepository.GetAllStores(name);
            return stores;
        }
        public async Task<Store> FindStoreById(Guid id)
        {
            var store = await _storeRepository.FindStoreById(id);
            return store;
        }
        public async Task<Store> CreateStore(Store store)
        {
           var newstore = await _storeRepository.CreateStore(store);
           return newstore;
        }
        public async Task<Store> UpdateStore(Store store, Guid oldStoreId)
        {
            var newstore = await _storeRepository.UpdateStore(store, oldStoreId);
            return newstore;
        }
        public async Task<bool> DeleteStore(Guid id)
        {
            var result = await _storeRepository.DeleteStore(id);
            return result;
        }

        public async Task<List<Store>> FindStoresByName(string name)
        {
            var store = await _storeRepository.FindStoresByName(name);
            return store;
        }
    }
}
