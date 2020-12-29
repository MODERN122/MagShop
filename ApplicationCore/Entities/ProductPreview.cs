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
        public double PriceNew { get; set; }
        public double PriceOld { get; set; }
        public string CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime DateEndDiscount { get; set; } = DateTime.Now;
        /// <summary>
        /// Constraint 0.0-5.0
        /// </summary>
        public double Rating { get; set; }
        /// <summary>
        /// Constraint 0.0-1.0 - weight for ML
        /// </summary>
        public virtual double Weight { get; set; }
    }
}
