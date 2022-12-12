using StoreService_AT.Model.Entities;
using StoreService_AT.Model.VOs;

namespace StoreService_AT.Repository
{
    public interface IStoreRepository
    {
        Task<List<Store>> GetAllStores();
        Task<Store> FindStoreById(Guid id);
        Task<Store> CreateStore(Store store);
        Task<Store> UpdateStore(Store store, Guid oldStoreId);
        Task<bool> DeleteStore(Guid id);
    }
}
