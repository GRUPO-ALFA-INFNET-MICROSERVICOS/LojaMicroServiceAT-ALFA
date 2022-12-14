using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StoreService_AT.Model.Entities
{
    public class Address
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        [JsonIgnore]
        public Guid StoreId { get; set; }

        [Required]
        public int Number { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string CEP { get; set; }
        [Required]
        public string Neighborhood { get; set; }
        public string Complement { get; set; }
    }
}
