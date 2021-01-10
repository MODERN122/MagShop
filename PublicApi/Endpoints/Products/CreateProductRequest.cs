using ApplicationCore.Entities;
using PublicApi.Base;
using System.Collections.Generic;

namespace PublicApi.Endpoints.Products
{
    public class CreateProductRequest : BaseRequest
    {
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string CategoryId { get; set; }
        public string Description { get; set; }
        public string StoreId { get; set; }
        public List<Property> Properties { get; set; }
    }
}