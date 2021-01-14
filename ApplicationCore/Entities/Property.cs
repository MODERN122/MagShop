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
        public PropertyItem(string caption)
        {
            Caption = caption;
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public byte[] Image { get; set; }
        public string Caption { get; set; }
    }
}