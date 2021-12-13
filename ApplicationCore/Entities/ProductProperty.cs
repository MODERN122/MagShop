using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class ProductProperty : IAggregateRoot
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime PublicationDate { get; set; } = DateTime.UtcNow;

        public string ProductId { get; set; }
        public Product Product { get; set; }

        public string PropertyId { get; set; }
        public Property Property { get; set; }

        public List<ProductPropertyItem> ProductPropertyItems { get; set; }
    }
}
