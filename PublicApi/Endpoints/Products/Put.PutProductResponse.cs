using ApplicationCore.Entities;
using PublicApi.Base;
using System;

namespace PublicApi.Endpoints.Products
{
    public class PutProductResponse: BaseResponse
    {
        public PutProductResponse(Guid guid) : base(guid)
        {

        }
        public Product Product { get; set; }
    }
}