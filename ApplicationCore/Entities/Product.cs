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
        /// <param name="productProperties">collection of Properties</param>
        /// <param name="storeId">identifier of Store</param>
        public Product(string id, string name, string categoryId, 
            string description, string storeId) 
        {
            Id = id;
            Name = name;
            CategoryId = categoryId;
            Description = description;
            StoreId = storeId;
            Random x = new Random();
            Rating = x.Next(1, 5);
        }
        public Product(string name, string categoryId,
            string description, string storeId)
        {
            Name = name;
            CategoryId = categoryId;
            Description = description;
            StoreId = storeId;

        }
        public List<Image> Images { get; set; }
        //Constraint 1.0-3.0
        public double Weight { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public List<Property> Properties { get; set; }
        public List<ProductProperty> ProductProperties { get; set; }
        //Not Added while
        //public List<string> Reviews { get; set; }

    }
}
