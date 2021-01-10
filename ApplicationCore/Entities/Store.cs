
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Store
    {
        public Store()
        {

        }
        public Store(string id, string sellerId, string name)
        {
            Id = id;
            SellerId = sellerId;
            Name = name;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public float Rating { get; set; }
        public List<Product> StoreProducts { get; set; }
        public string SellerId { get; set; }
        [JsonIgnore]
        public User Seller { get; set; }


    }
}
