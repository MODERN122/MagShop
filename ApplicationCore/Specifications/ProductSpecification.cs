using ApplicationCore.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class ProductSpecification : Specification<Product>
    {
        public ProductSpecification(string productId)
        {
            Query
                .Where(p => p.ProductId == productId)
                .Include(i => i.Category);
            //Query.Include(q => q.Properties);
        }
        public ProductSpecification(string categoryId, string storeId, List<string> properties, int pageIndex = 0, int pageSize = 20)
        {
            Query
                .Where(p => (string.IsNullOrWhiteSpace(categoryId) || p.CategoryId == categoryId) && (string.IsNullOrWhiteSpace(storeId) || p.StoreId == storeId));
            if (properties != null)
            {
                Query.Where(p => p.Properties.Any(a => properties.Contains(a.Id)));
            }
            Query
                .Skip(pageSize * pageIndex)
                .Take(pageSize);
            Query
                .Include(i => i.Category);
            Query.Include(p => p.Properties)
                .ThenInclude(i => i.PropertyItems);
        }
    }
}
