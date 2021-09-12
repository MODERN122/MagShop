using ApplicationCore.Entities;
using ApplicationCore.Base;
using System;

namespace ApplicationCore.RESTApi.Products
{
    public class PutProductResponse: BaseResponse
    {
        public PutProductResponse(Guid guid) : base(guid)
        {

        }
        public Product Product { get; set; }
    }
}