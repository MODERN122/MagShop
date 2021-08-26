using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ApplicationCore.Entities
{
    public class ProductProperty
    {
        [JsonIgnore]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime PublicationDate { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }

        public string PropertyId { get; set; }
        [JsonIgnore]
        public Property Property { get; set; }

        public List<ProductPropertyItem> ProductPropertyItems { get; set; }
    }
}
