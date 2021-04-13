using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ApplicationCore.Entities
{
    public class ProductPreview : IAggregateRoot
    {
        public ProductPreview()
        {

        }
        public string ProductId { get; set; } = Guid.NewGuid().ToString();
        public string ProductName { get; set; }
        public byte[] PreviewImage { get; set; }
        public double? PriceNew { get; set; }
        public double? PriceOld { get; set; }
        public string CategoryId { get; set; }

        public Category Category { get; set; }
        public DateTime? DateEndDiscount { get; set; } = DateTime.Now;
        /// <summary>
        /// Constraint 0.0-5.0
        /// </summary>
        public double Rating { get; set; }
        public List<Property> Properties { get; set; }
        [Required]
        public string StoreId { get; set; }
        [JsonIgnore]
        public Store Store { get; set; }
    }
}
