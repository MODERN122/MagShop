using ApplicationCore.Entities;
using ApplicationCore.Base;
using System;

namespace ApplicationCore.RESTApi.Products
{
    public class GetProductResponse:BaseResponse
    {
        public Product Product { get; set; }
    }
}