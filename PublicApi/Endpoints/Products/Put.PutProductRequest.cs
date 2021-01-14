
using ApplicationCore.Entities;
using PublicApi.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PublicApi.Endpoints.Products
{
    public class PutProductRequest : BaseRequest
    {
        [Required]
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public Image PreviewImage { get; set; }
        public double? PriceNew { get; set; }
        public double? PriceOld { get; set; }
        public string CategoryId { get; set; }
        public DateTime? DateEndDiscount { get; set; }
        public List<Image> Images { get; set; }

        public string Description { get; set; }
        //Not Added while
        //public List<string> Reviews { get; set; }
        public List<Property> Properties { get; set; }
    }
}