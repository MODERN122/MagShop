using ApplicationCore.Interfaces;
using System;

namespace ApplicationCore.Entities
{
    public class ProductImage : IAggregateRoot
    {
        [Obsolete("Uses only for EfCore generating")]
        public ProductImage() { }
        public ProductImage(string productId, string path)
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
