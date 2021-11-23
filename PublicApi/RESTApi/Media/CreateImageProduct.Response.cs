using ApplicationCore.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.RESTApi.Media
{
    public class CreateImageProductResponse : BaseResponse
    {
        public CreateImageProductResponse(Guid guid) : base(guid)
        {

        }
        public string ImagePath { get; set; }
    }
}
