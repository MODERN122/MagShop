using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Interfaces;
using System.Text.Json.Serialization;

namespace ApplicationCore.Entities
{
    public class Product: ProductPreview
    {
        public Product()
        {

        }
        /// <summary>
        /// Initialize Product
        /// </summary>
        /// <param name="id">identifier</param>
        /// <param name="name">name of Product</param>
        /// <param name="price">price new of Product</param>
        /// <param name="categoryId">identifier of Category</param>
        /// <param name="description">description</param>
        /// <param name="properties">collection of Properties</param>
        /// <param name="storeId">identifier of Store</param>
        public Product(string id, string name, double price, string categoryId, 
            string description, List<Property> properties, string storeId) 
        {
            ProductId = id;
            ProductName = name;
            PriceNew = price;
            CategoryId = categoryId;
            Description = description;
            Properties = properties;
            StoreId = storeId;
        }
        public Product(string name, double price, string categoryId,
            string description, List<Property> properties, string storeId)
        {
            ProductName = name;
            PriceNew = price;
            CategoryId = categoryId;
            Description = description;
            Properties = properties;
            StoreId = storeId;
        }
        public List<Image> Images { get; set; }
        //Constraint 1.0-3.0
        public double Weight { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Url { get; set; }
        //Not Added while
        //public List<string> Reviews { get; set; }

    }
}
