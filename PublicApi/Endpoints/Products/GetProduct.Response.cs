using ApplicationCore.Entities;
using PublicApi.Base;
using System;

namespace PublicApi.Endpoints.Products
{
    public class GetProductResponse:BaseResponse
    {
        public Product Product { get; set; }
    }
}