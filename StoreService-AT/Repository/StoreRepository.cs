using AutoMapper;
using StoreService_AT.Model;
using StoreService_AT.Model.VOs;

namespace StoreService_AT.Repository
{
    public class StoreRepository : IStoreRepository
    {
        static List<Store> storeList = new List<Store>();
        private IMapper _mapper;
        public async Task<List<Store>> GetAllStores()
        {
            return await  Task.FromResult(storeList);
            
        }
        public async Task<Store> FindStoreById(Guid id)
        {
            var store = Task.FromResult(storeList.Where(x => x.Id == id).FirstOrDefault());
            return await store;
        }
        public async Task<Store> CreateStore(Store store)
        {
            storeList.Add(store);
            return await Task.FromResult(store);
        }
        public async Task<Store> UpdateStore(Store store, Guid oldStoreId)
        {
            foreach (Store oldStore in storeList)
            {
                if (oldStore.Id == oldStoreId)
                {
                    if (oldStore.StoreName != "")
                    {
                        oldStore.StoreName = store.StoreName;
                    }
                    if (store.Telephone != "")
                    {
                        oldStore.Telephone = store.Telephone;
                    }
                    if (store.StoreAdress != null)
                    {
                        oldStore.StoreAdress = store.StoreAdress;
                    }
                    if (store.Products != null)
                    {
                        oldStore.Products = store.Products;
                    }
                    return await Task.FromResult(store);

                }
            }
            return null;
        }
        public async Task<bool> DeleteStore(Guid id)
        {
            var store = FindStoreById(id).Result;
            storeList.Remove(store);
            return await Task.FromResult(true);
        }




    }
}
