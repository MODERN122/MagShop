using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApplicationCore.Entities
{
    public class ProductPreview : IAggregateRoot
    {
        public ProductPreview()
        {

        }
        public string ProductId { get; set; } = Guid.NewGuid().ToString();
        public string ProductName { get; set; }
        public Image PreviewImage { get; set; }
        public double? PriceNew { get; set; }
        public double? PriceOld { get; set; }
        public string CategoryId { get; set; }

        public Category Category { get; set; }
        public DateTime? DateEndDiscount { get; set; } = DateTime.Now;
        /// <summary>
        /// Constraint 0.0-5.0
        /// </summary>
        public double Rating { get; set; }
    }
}
