using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StoreService_AT.Controllers;
using StoreService_AT.Model.Entities;
using StoreService_AT.Model.ValueObjects;
using StoreService_AT.Model.VOs;
using StoreService_AT.Repository;
using StoreService_AT.Service.ProductService;
using StoreService_AT.Service.StoreService;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit.Sdk;

namespace TestStoreService
{
    public class StoreAPITest
    {
        private IStoreDatabaseSettings MockCriarDatabaseSettings()
        {
            Mock<IStoreDatabaseSettings> mockObject1 = new Mock<IStoreDatabaseSettings>();
            mockObject1.Setup(m => m.ConnectionString).Returns("mongodb+srv://natanAdmin:natanAdmin21@clusterstore.0ccionx.mongodb.net/?retryWrites=true&w=majority");
            mockObject1.Setup(m => m.StoreCollectionName).Returns("Store");
            mockObject1.Setup(m => m.DatabaseName).Returns("StoreDB");
            return mockObject1.Object;
        }
        #region Teste de Controller
        [Fact]
        public void TestGetStore()
        {
            //IProductService productService = this.prvGetMockProductRepositoryGetAll();
            IStoreDatabaseSettings databaseSettings = this.MockCriarDatabaseSettings();
            StoreRepository storeRepository = new StoreRepository(databaseSettings);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://product.bloco.live/");
            ProductService productService = new ProductService(client);
            StoreService storeService = new StoreService(storeRepository);
            StoreController storeController = new StoreController(storeService, productService);

            var actualResult = storeController.GetStores("","","",1,15);
            var actualdata = actualResult.Result;
            Assert.NotNull(actualdata);
            Assert.True(actualResult.IsCompletedSuccessfully);
        }
        [Fact]
        public Guid TestGetStoreById()
        {
            IStoreDatabaseSettings databaseSettings = this.MockCriarDatabaseSettings();
            //IProductService productService = this.prvGetMockProductRepositoryGetAll();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://product.bloco.live/");
            StoreRepository storeRepository = new StoreRepository(databaseSettings);
            ProductService productService = new ProductService(client);
            StoreService storeService = new StoreService(storeRepository);
            StoreController storeController = new StoreController(storeService, productService);

            var allStores =  storeController.GetStores("","", "", 1, 15);
            var actualdata = (allStores.Result.Result as OkObjectResult).Value as List<Store>;

            var storeToFind = actualdata.Select(x => x.Id).Last();

            var actualResult =  storeController.GetById(storeToFind, "", "", 1, 15);
            var actualdataTest = (actualResult.Result.Result as OkObjectResult).Value as Store;
            Assert.NotNull(actualdataTest);
            return actualdataTest.Id;
        }
        [Fact]
        public async void TestPostStore()
        {
            IStoreDatabaseSettings databaseSettings = this.MockCriarDatabaseSettings();
            //IProductService productService = this.prvGetMockProductRepositoryGetAll();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://product.bloco.live/");
            StoreRepository storeRepository = new StoreRepository(databaseSettings);
            ProductService productService = new ProductService(client);
            StoreService storeService = new StoreService(storeRepository);
            StoreController storeController = new StoreController(storeService, productService);


            var store = new StoreVo()
            {
                Id = Guid.NewGuid(),
                StoreName = "Teste Criado pro delete",
                Telephone = "9782132485",
            };
            var address = new Address()
            {
                Street = "rua aleatoria do teste",
                CEP = "88841-221",
                Neighborhood = "Jardim do teste",
                Id = Guid.NewGuid(),
                Complement = "Apartamento 120",
                Number = 15,
                StoreId = store.Id
            };
            store.StoreAdress = address;

            var actualResult = await storeController.Create(store);
            var actualdata = (actualResult.Result as OkObjectResult).Value as Store;
            Assert.NotNull(actualdata);
            TestDeleteStore();
        }
        [Fact]
        public void TestDeleteStore()
        {
            IStoreDatabaseSettings databaseSettings = this.MockCriarDatabaseSettings();
            //IProductService productService = this.prvGetMockProductRepositoryGetAll();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://product.bloco.live/");
            StoreRepository storeRepository = new StoreRepository(databaseSettings);
            ProductService productService = new ProductService(client);
            StoreService storeService = new StoreService(storeRepository);
            StoreController storeController = new StoreController(storeService, productService);

            var actualresult2 = storeController.Delete(TestGetStoreById());
            Assert.NotNull(actualresult2.Result);
            Assert.True(actualresult2.IsCompletedSuccessfully);
        }
        [Fact]
        public void TestEditStore()
        {
            IStoreDatabaseSettings databaseSettings = this.MockCriarDatabaseSettings();
            //IProductService productService = this.prvGetMockProductRepositoryGetAll();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://product.bloco.live/");
            StoreRepository storeRepository = new StoreRepository(databaseSettings);
            ProductService productService = new ProductService(client);
            StoreService storeService = new StoreService(storeRepository);
            StoreController storeController = new StoreController(storeService, productService);


            var store = new StoreVo()
            {
                StoreName = "Teste Para Editar",
                Telephone = "874987421",
            };
            var address = new Address()
            {
                Street = "rua aleatoria do teste editado",
                CEP = "88841-221",
                Neighborhood = "Jardim do teste editado",
                Id = Guid.NewGuid(),
                Complement = "Apartamento 120",
                Number = 15,
                StoreId = store.Id
            };
            store.StoreAdress = address;

            var actualResult = storeController.Edit(store, TestGetStoreById());
            var actualdata = actualResult.Result;
            Assert.NotNull(actualdata);
            Assert.True(actualResult.IsCompletedSuccessfully);
        }
        #endregion
        #region Teste Unitario
        #endregion
    }
}