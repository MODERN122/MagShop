using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.RESTApi.Media
{
    public class CreateImageProductRequest
    {
        public string ImageName { get; set; }
        public IFormFile File { get; set; }
        public string ProductId { get; set; }
    }
}
