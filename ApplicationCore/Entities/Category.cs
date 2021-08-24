
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Category
    {
        /// <summary>
        /// Get category with parent Category
        /// </summary>
        /// <param name="id">identifier category</param>
        /// <param name="parentId">identidier category</param>
        /// <param name="name">name of category</param>
        /// <param name="weight">weight for ML constraint 0.0-1.0</param>
        public Category(string id, string parentId, string name, double weight)
        {
            Id = id;
            ParentId = parentId;
            Name = name;
            Weight = weight;
        }
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Constraint 0.0-1.0
        /// </summary>
        public double Weight { get; set; }
        [JsonIgnore]
        public Category ParentCategory { get; set; }
        [JsonIgnore]
        public List<Category> Childs { get; set; }
        [JsonIgnore]
        public List<Product> Products { get; set; }

        public Category()
        {

        }
    }
}
