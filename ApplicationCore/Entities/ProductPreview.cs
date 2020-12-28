using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApplicationCore.Entities
{
    public class ProductPreview
    {
        public ProductPreview()
        {

        }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUri { get; set; }
        public float PriceNew { get; set; }
        public float PriceOld { get; set; }
        public string CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime DateEndDiscount { get; set; } = DateTime.Now;
        //Constraint 0.0-5.0
        public float Rating { get; set; }
        //Constraint 0.0-1.0 - weight for ML
        public virtual float Weight { get; set; }
    }
}
