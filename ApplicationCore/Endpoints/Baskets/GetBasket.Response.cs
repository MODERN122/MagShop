using ApplicationCore.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;

namespace ApplicationCore.RESTApi.Baskets
{
    public class GetBasketResponse : BaseResponse
    {
        public GetBasketResponse(Guid guid) : base(guid)
        {

        }
        public string BasketId { get; set; }
        public List<BasketItemResponse> BasketItems { get; set; }
    }
    public class BasketItemResponse
    {        
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public ProductPreview Product { get; set; }
    }
}
