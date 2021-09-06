using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities
{
    public class Property : BaseDateTimeEntity
    {
        public Property()
        {

        }
        public Property(string id, string propertyName, List<PropertyItem> propertyItems)
        {
            Id = id;
            Name = propertyName;
            Items = propertyItems;
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public List<PropertyItem> Items { get; set; }
        public ICollection<Product> Products { get; set; }
        public List<ProductProperty> ProductProperties { get; set; }
    }
    public class ProductPropertyItem : BaseDateTimeEntity
    {
        public ProductPropertyItem(string caption, double priceNew, double priceOld)
        {
            Caption = caption;
            PriceNew = priceNew;
            PriceOld = priceOld;
        }
        /// <summary>
        /// Only for seed data
        /// </summary>
        /// <param name="caption">Caption for labeling propertyItem</param>
        public ProductPropertyItem(string propertyItemId, string caption)
        {
            Random random = new Random();
            Caption = caption;
            PriceNew = random.Next(1000, 10000);
            PriceOld = PriceNew + random.Next(100, 999);
            PropertyItemId = propertyItemId;
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ProductPropertyId { get; set; }
        public string PropertyItemId { get; set; }
        public PropertyItem PropertyItem { get; set; }
        [ForeignKey("ProductPropertyId")]
        public ProductProperty ProductProperty { get; set; }
        public byte[] Image { get; set; } = new byte[0];

        public double PriceNew { get; set; } = -1;
        public double? PriceOld { get; set; }
        public string Caption { get; set; }
    }
    public class PropertyItem
    {
        public PropertyItem() { }
        public PropertyItem(string id, string caption)
        {
            Id = id;
            Caption = caption;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Caption { get; set; }
        //TODO add default image
        public string PropertyId { get; set; }
        //Dont Use
        public ICollection<ProductPropertyItem> ProductPropertyItems { get; set; }
    }

}