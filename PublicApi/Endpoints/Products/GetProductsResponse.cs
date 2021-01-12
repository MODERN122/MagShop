using ApplicationCore.Entities;
using PublicApi.Base;
using System;
using System.Collections.Generic;

namespace PublicApi.Endpoints.Products
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