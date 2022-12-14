using StoreService_AT.Model.Entities;

namespace StoreService_AT.Service.StoreService
{
    public interface IStoreService
    {
        Task<List<Store>> GetAllStores(string name);
        Task<Store> FindStoreById(Guid id);
        Task<List<Store>> FindStoresByName(string name);
        Task<Store> CreateStore(Store store);
        Task<Store> UpdateStore(Store store, Guid oldStoreId);
        Task<bool> DeleteStore(Guid id);
    }
}
