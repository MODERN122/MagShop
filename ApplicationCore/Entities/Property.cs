using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Property
    {
        public Property()
        {

        }
        public Property(string propertyName, List<PropertyItem> propertyItems)
        {
            PropertyName = propertyName;
            PropertyItems = propertyItems;
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PropertyName { get; set; }
        public List<PropertyItem> PropertyItems {get;set;}
    }
    public class PropertyItem
    {
        public PropertyItem()
        {

        }
        public PropertyItem(string caption, double priceNew)
        {
            Caption = caption;
            PriceNew = priceNew;
        }
        /// <summary>
        /// Only for seed data
        /// </summary>
        /// <param name="caption">Caption for labeling propertyItem</param>
        public PropertyItem(string caption)
        {
            Random random = new Random();
            Caption = caption;
            PriceNew = random.Next(1000, 10000);
            PriceOld = PriceNew + random.Next(100, 999);
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public byte[] Image { get; set; }
        public double? PriceNew { get; set; }
        public double? PriceOld { get; set; }
        public string Caption { get; set; }
    }
}