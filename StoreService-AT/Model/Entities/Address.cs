﻿namespace StoreService_AT.Model.Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        public Guid StoreId { get; set; }
        public int Number { get; set; }
        public string Street { get; set; }
        public string CEP { get; set; }
        public string Neighborhood { get; set; }
        public string Complement { get; set; }
    }
}
