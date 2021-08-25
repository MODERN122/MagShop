using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class ProductProperty
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime PublicationDate { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }

        public string PropertyId { get; set; }
        public Property Property { get; set; }

        public List<ProductPropertyItem> ProductPropertyItems { get; set; }
    }
}
