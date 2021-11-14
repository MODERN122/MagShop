using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities
{
    public class Property : BaseDateTimeEntity
    {
        [Obsolete("Uses only for EF Core generating")]
        public Property()
        {

        }
        public Property(string id, string propertyName, List<PropertyItem> propertyItems, string userId) : base(userId)
        {
            Id = id;
            Name = propertyName;
            Items = propertyItems;
        }
        public Property(string propertyName, List<PropertyItem> propertyItems, string userId) : base(userId)
        {
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
        [Obsolete("Uses only for EF Core generating")]
        public ProductPropertyItem() { }
        public ProductPropertyItem(string caption, double priceNew, double priceOld, string userId) : base(userId)
        {
            Caption = caption;
            PriceNew = priceNew;
            PriceOld = priceOld;
        }
        public ProductPropertyItem(string propertyItemId, string caption, double priceNew, string imagePath, string userId) : base(userId)
        {
            PropertyItemId = propertyItemId;
            Caption = caption;
            PriceNew = priceNew;
            ImagePath = imagePath;
        }

        /// <summary>
        /// Only for seed data
        /// </summary>
        /// <param name="caption">Caption for labeling propertyItem</param>
        public ProductPropertyItem(string propertyItemId, string caption, string userId) : base(userId)
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
        public string ImagePath { get; set; }
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