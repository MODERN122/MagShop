using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMarket.Models
{
    public class CreateProductRequest
    {
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string CategoryId { get; set; }
        public string Description { get; set; }
        public string StoreId { get; set; }
        public List<Property> Properties { get; set; }
    }
}
