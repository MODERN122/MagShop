using ApplicationCore.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApplicationCore.Endpoints.Baskets
{
    public class GetBasketRequest : BaseRequest
    {
        [Required]
        [JsonPropertyName("id")]
        public string UserId { get; set; }
    }
}
