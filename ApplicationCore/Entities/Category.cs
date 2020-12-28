using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Category
    {
        public string CategoryId { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        //Constraint 0.0-1.0
        public float Weight { get; set; }
        public Category ParentCategory { get; set; }
        public List<Category> ChildrenCategories { get; set; }
        public List<Product> Products { get; set; }

        public Category()
        {

        }
    }
}
