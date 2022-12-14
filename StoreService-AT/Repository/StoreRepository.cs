using AutoMapper;
using MongoDB.Driver;
using StoreService_AT.Model.Entities;
using StoreService_AT.Model.VOs;

namespace StoreService_AT.Repository
{
    public class StoreRepository : IStoreRepository
    {
        private IMapper _mapper;
        private readonly IMongoCollection<Store> _store;


        public StoreRepository(IStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _store = database.GetCollection<Store>(settings.StoreCollectionName);
        }

        public async Task<List<Store>> GetAllStores(string name)
        {
            return await Task.FromResult(_store.Find(store => store.StoreName.StartsWith(name)).ToList());

        }
        public async Task<Store> FindStoreById(Guid id)
        {
            var store = await Task.FromResult(_store.Find(store => store.Id == id).FirstOrDefault());
            return store;
        }
        public async Task<Store> CreateStore(Store store)
        {
            _store.InsertOne(store);
            return await Task.FromResult(store);
        }
        public async Task<Store> UpdateStore(Store Newstore, Guid oldStoreId)
        {
            var oldStore = FindStoreById(oldStoreId).Result;
            Newstore.Id = oldStore.Id;
            Newstore.StoreAdress.Id = oldStore.StoreAdress.Id;
            Newstore.StoreAdress.StoreId = oldStore.StoreAdress.StoreId;
            _store.ReplaceOne(store => store.Id == oldStoreId, Newstore);
            return await Task.FromResult(Newstore);
        }
        public async Task<bool> DeleteStore(Guid id)
        {

            var result = _store.DeleteOne(store => store.Id == id).IsAcknowledged;
            return await Task.FromResult(result);
        }

        public async Task<List<Store>> FindStoresByName(string name)
        {
            var store = await Task.FromResult(_store.Find(store => store.StoreName.StartsWith(name)).ToList());
            return store;
        }
    }
}
