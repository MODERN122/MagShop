using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ApplicationCore.Entities
{
    public class Image
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ProductId { get; set; }
        public byte[] ByteImage { get; set; }
    }
}
