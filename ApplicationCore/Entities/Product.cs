using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities
{
    public class Product: ProductPreview
    {
        public Product()
        {

        }
        public string[] ImagesUri { get; set; }
        //Constraint 1.0-3.0
        public override float Weight { get; set; }

        public string Description { get; set; }
        //Not Added while
        //public List<string> Reviews { get; set; }
        public string StoreId { get; set; }
        public Store Store { get; set; }
        public List<Property> Properties { get; set; }

    }
}
