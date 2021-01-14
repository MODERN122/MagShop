using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ApplicationCore.Entities
{
    public class Image
    {
        public Guid Id { get; set; }
        public string ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
        public byte[] byteImage { get; set; }
    }
}
