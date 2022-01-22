using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApplicationCore.Entities
{
    public class ProductPreview : BaseDateTimeEntity
    {
        [Obsolete("Uses only for EF Core generating")]
        public ProductPreview()
        {                                      

        }
        public ProductPreview(string userId) :base(userId) { }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Image { get; set; }
        public string CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime DateEndDiscount { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// Constraint 0.0-5.0
        /// </summary>
        public double Rating { get; set; }
        [Required]
        public string StoreId { get; set; }
        [JsonIgnore]
        public Store Store { get; set; }
    }
}
