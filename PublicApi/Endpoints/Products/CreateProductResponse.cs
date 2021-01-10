using ApplicationCore.Entities;
using PublicApi.Base;
using System;

namespace PublicApi.Endpoints.Products
{
    public class CreateProductResponse : BaseResponse
    {
        public CreateProductResponse(Guid guid) : base(guid)
        {

        }
        public Product Product { get; set; }
    }
}