﻿namespace StoreService_AT.Model.Entities
{
    public class StoreDatabaseSettings : IStoreDatabaseSettings
    {
        public string StoreCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IStoreDatabaseSettings
    {
        string StoreCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
