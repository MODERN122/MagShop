using ApplicationCore.Base;
using System.Collections.Generic;

namespace ApplicationCore.Endpoints.Products
{
    public class GetProductsRequest: BaseRequest
    {
        public string CategoryId { get; set; }
        public string StoreId { get; set; }
        public List<string> PropertiesId { get; set; } 
        public int? PageSize { get; set; }
        public int? PageIndex { get; set; }
    }
}