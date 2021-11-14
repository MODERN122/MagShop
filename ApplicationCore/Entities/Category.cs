
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities
{
    public class Category : BaseDateTimeEntity
    {
        /// <summary>
        /// Get category with parent Category
        /// </summary>
        /// <param name="id">identifier category</param>
        /// <param name="parentId">identidier category</param>
        /// <param name="name">name of category</param>
        /// <param name="weight">weight for ML constraint 0.0-1.0</param>
        public Category(string id, string parentId, string name, double weight, string userId):base (userId)
        {
            Id = id;
            ParentId = parentId;
            Name = name;
            Weight = weight;
        }
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; } = "https://www.clipartkey.com/mpngs/m/150-1501297_f" +
            "inish-clipart-icon-png-product-icon-ico.png";
        /// <summary>
        /// Constraint 0.0-1.0
        /// </summary>
        public double Weight { get; set; }
        public Category ParentCategory { get; set; }
        public List<Category> Childs { get; set; }
        public List<Product> Products { get; set; }

        [Obsolete("Uses only for EF Core generating")]
        public Category() { }
        public Category(string userId):base(userId)
        {

        }
    }
}
