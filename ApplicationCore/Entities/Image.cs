using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ApplicationCore.Entities
{
    public class Image : IAggregateRoot
    {
        [Obsolete("Uses only for EfCore generating")]
        public Image() { }
        public Image(string productId, string path)
        {
            ProductId = productId;
            Path = path;
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ProductId { get; set; }
        public string Path { get; set; }
        public DateTime UploadDateTime { get; set; } = DateTime.UtcNow;
    }
}
