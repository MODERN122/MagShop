using ApplicationCore.Entities;
using ApplicationCore.Base;
using System.Collections.Generic;

namespace ApplicationCore.RESTApi.Products
{
    public class CreateProductRequest : BaseRequest
    {
        public string ProductName { get; set; }
        public string CategoryId { get; set; }
        public string Description { get; set; }
        public string StoreId { get; set; }
        public string Url { get; set; }
        public byte[] PreviewImage { get; set; }
        public List<Property> Properties { get; set; }
    }
}