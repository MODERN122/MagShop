﻿using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace CloudMarket.Models
{
    public class PutProductRequest
    {
        [Required]
        [JsonPropertyName("id")]
        public string Id { get; set; }
        public string ProductName { get; set; }
        public Image PreviewImage { get; set; }
        public double? PriceNew { get; set; }
        public double? PriceOld { get; set; }
        public string CategoryId { get; set; }
        public DateTime? DateEndDiscount { get; set; }
        public List<Image> Images { get; set; }
        public string Description { get; set; }
        public List<Property> Properties { get; set; }
    }
}
