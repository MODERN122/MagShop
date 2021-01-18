using ApplicationCore.Entities;
using ApplicationCore.Base;
using System;

namespace ApplicationCore.Endpoints.Products
{
    public class GetProductResponse:BaseResponse
    {
        public Product Product { get; set; }
    }
}