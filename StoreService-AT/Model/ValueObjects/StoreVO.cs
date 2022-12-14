using StoreService_AT.Model.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StoreService_AT.Model.VOs
{
    public class StoreVo
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        [Required]
        public string StoreName { get; set; }
        [Required]
        public string Telephone { get; set; }
        [Required]
        public Address StoreAdress { get; set; }
    }
}
