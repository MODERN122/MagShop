using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Interfaces;
using System.Text.Json.Serialization;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Product : ProductPreview
    {
        [Obsolete("Uses only for EF Core generating")]
        public Product()
        {

        }
        public Product(string userId) : base(userId) { }
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
            string description, string storeId, List<ProductProperty> productProperties, string userId):base(userId)
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
        /// <summary>
        /// Add pre publish product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="categoryId"></param>
        /// <param name="description"></param>
        /// <param name="storeId"></param>
        public Product(string name, string categoryId,
           string description, string storeId, string userId) : base(userId)
        {
            Name = name;
            CategoryId = categoryId;
            Description = description;
            StoreId = storeId;
            Random x = new Random();
            Rating = x.Next(1, 5);
        }
        public Product(string name, string categoryId,
            string description, string storeId, List<ProductProperty> productProperties, string userId) : base(userId)
        {
            Name = name;
            CategoryId = categoryId;
            Description = description;
            StoreId = storeId;
            SetProductProperties(productProperties);
        }
        public bool IsActive { get; private set; } = false;
        public List<ProductImage> Images { get; set; }
        public string PriceNew { get; private set; }
        public string PriceOld { get; private set; }
        public int Discount { get; private set; }
        //Constraint 1.0-3.0
        public double Weight { get; set; }
        public string Description { get; set; }
        public List<Property> Properties { get; set; }
        public List<ProductProperty> ProductProperties { get; private set; }
        public List<ChoosenProduct> ChoosenProducts { get; private set; }
        //Not Added while
        //public List<string> Reviews { get; set; }
        public void SetProductIsActive(bool isActive)
        {
            IsActive = isActive;
        }

        public bool SetProductProperties(List<ProductProperty> productProperties)
        {
            ProductProperties = productProperties;

            PriceOld = $"{ProductProperties.Select(x => x.ProductPropertyItems.Min(y => y.PriceOld)).Sum()}" +
            $" - {ProductProperties.Select(x => x.ProductPropertyItems.Max(y => y.PriceOld)).Sum()}";
            if (PriceOld == "0 - 0")
            {
                PriceOld = "";
            }
            PriceNew = $"{ProductProperties.Select(x => x.ProductPropertyItems.Min(y => y.PriceNew)).Sum()}" +
            $" - {ProductProperties.Select(x => x.ProductPropertyItems.Max(y => y.PriceNew)).Sum()}";
            if (PriceNew == "0 - 0")
            {
                PriceNew = "";
                return false;
            }

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
            if (priceOld <= priceNew || priceOld == 0)
            {
                Discount = 0;
            }
            else
            {
                var discount = (priceOld - priceNew) * 100 / priceOld;
                Discount = (int)(discount > 0 ? discount : 0);
            }
            return true;
        }

    }
}
