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
        public string Id { get; set; }
        public float Rating { get; set; }
        public List<Product> StoreProducts { get; set; }
        public string SellerId { get; set; }
        public User Seller { get; set; }


    }
}
