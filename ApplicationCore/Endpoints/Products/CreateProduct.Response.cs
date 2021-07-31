using ApplicationCore.Entities;
using ApplicationCore.Base;
using System;

namespace ApplicationCore.RESTApi.Products
{
    public class CreateProductResponse : BaseResponse
    {
        public CreateProductResponse(Guid guid) : base(guid)
        {

        }
        public Product Product { get; set; }
    }
}