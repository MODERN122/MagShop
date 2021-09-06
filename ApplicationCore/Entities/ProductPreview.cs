using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ApplicationCore.Entities
{
    public class ProductPreview : BaseDateTimeEntity
    {
        public ProductPreview()
        {

        }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Image { get; set; } = "https://sun9-54.userapi.com/impg/AiOjSFgShn1Z2PYCIfbb_re4Ivpc3A132Unu2w/qI6EEjgHAd0.jpg" +
            "?size=1439x2160&quality=96&sign=3263d89b63287df39a08354aa5269bb4&type=album";
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
