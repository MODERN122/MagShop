using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Property
    {
        public string Id { get; set; }
        public string PropertyName { get; set; }
        public List<PropertyItem> PropertyItems {get;set;}
    }
    public class PropertyItem
    {
        public string Id { get; set; }
        public byte[] Image { get; set; }
        public string Caption { get; set; }
    }
}