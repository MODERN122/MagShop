
using ApplicationCore.Entities;
using ApplicationCore.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApplicationCore.RESTApi.Products
{
    public class PutProductRequest : BaseRequest
    {
        [Required]
        [JsonPropertyName("id")]
        public string Id { get; set; }
        public string ProductName { get; set; }
        public ProductImage PreviewImage { get; set; }
        public double? PriceNew { get; set; }
        public double? PriceOld { get; set; }
        public string CategoryId { get; set; }
        public DateTime? DateEndDiscount { get; set; }
        public List<ProductImage> Images { get; set; }
        public string Description { get; set; }
        public List<Property> Properties { get; set; }
    }
}