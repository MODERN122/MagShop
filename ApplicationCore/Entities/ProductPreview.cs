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
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string PreviewImagePath { get; set; }
        public string CategoryId { get; set; }

        public Category Category { get; set; }
        public DateTime? DateEndDiscount { get; set; } = DateTime.Now;
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
