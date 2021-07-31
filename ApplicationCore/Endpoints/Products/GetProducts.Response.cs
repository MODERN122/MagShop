using ApplicationCore.Entities;
using ApplicationCore.Base;
using System;
using System.Collections.Generic;

namespace ApplicationCore.RESTApi.Products
{
    public class GetProductsResponse : BaseResponse
    {
        public GetProductsResponse()
        {
        }

        public GetProductsResponse(Guid guid) : base(guid)
        {

        }
        public List<ProductPreview> Products { get; set; }
    }
}