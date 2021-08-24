using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class ProductProperty
    {
        public DateTime PublicationDate { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }

        public string PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
