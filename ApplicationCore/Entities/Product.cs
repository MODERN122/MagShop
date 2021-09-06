using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Interfaces;
using System.Text.Json.Serialization;
using System.Linq;

namespace ApplicationCore.Entities
{
    public class Product : ProductPreview
    {
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
            string description, string storeId, List<ProductProperty> productProperties)
        {
            Id = id;
            Name = name;
            CategoryId = categoryId;
            Description = description;
            StoreId = storeId;
            Random x = new Random();
            Rating = x.Next(1, 5);
            SetProductProperties(productProperties);
        }
        public Product(string name, string categoryId,
            string description, string storeId, List<ProductProperty> productProperties)
        {
            Name = name;
            CategoryId = categoryId;
            Description = description;
            StoreId = storeId;
            SetProductProperties(productProperties);
        }
        public List<Image> Images { get; set; }
        public string PriceNew { get; private set; }
        public string PriceOld { get; private set; }
        public int Discount { get; private set; }
        //Constraint 1.0-3.0
        public double Weight { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public List<Property> Properties { get; set; }
        public List<ProductProperty> ProductProperties { get; private set; }
        //Not Added while
        //public List<string> Reviews { get; set; }

        public void SetProductProperties(List<ProductProperty> productProperties)
        {
            ProductProperties = productProperties;

            PriceOld = $"{ProductProperties.Select(x => x.ProductPropertyItems.Min(y => y.PriceOld)).Sum()}" +
            $" - {ProductProperties.Select(x => x.ProductPropertyItems.Max(y => y.PriceOld)).Sum()}";
            PriceNew = $"{ProductProperties.Select(x => x.ProductPropertyItems.Min(y => y.PriceNew)).Sum()}" +
            $" - {ProductProperties.Select(x => x.ProductPropertyItems.Max(y => y.PriceNew)).Sum()}";


            double priceNew = 0;
            double priceOld = 0;
            foreach (var productProperty in ProductProperties)
            {
                double tempPriceNew = 0;
                double tempPriceOld = double.MaxValue;
                foreach (var productPropertyItem in productProperty.ProductPropertyItems)
                {
                    if (productPropertyItem.PriceOld == null)
                    {
                        tempPriceNew = 0;
                        tempPriceOld = 0;
                        break;
                    }
                    if (productPropertyItem.PriceOld - productPropertyItem.PriceNew < tempPriceOld - tempPriceNew)
                    {
                        tempPriceNew = productPropertyItem.PriceNew;
                        tempPriceOld = productPropertyItem.PriceOld.Value;
                    }
                }
                priceNew += tempPriceNew;
                priceOld += tempPriceOld;
            }
            Discount = (int)((priceOld - priceNew) * 100 / priceOld);
        }

    }
}
