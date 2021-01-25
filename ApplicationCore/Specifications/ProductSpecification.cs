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
                .Where(p => p.ProductId == productId);
            Query
                .Include(i => i.Category);
            Query.Include(p => p.Properties)
                .ThenInclude(i => i.PropertyItems);
            Query.
                Include(p => p.Images);
            Query.Include(p => p.Store);
        }
        public ProductSpecification(string categoryId, string storeId, List<string> properties, int pageIndex = 0, int pageSize = 20)
        {
            Query
                .Where(p => (string.IsNullOrWhiteSpace(categoryId) || p.CategoryId == categoryId) && (string.IsNullOrWhiteSpace(storeId) || p.StoreId == storeId));
            if (properties != null && properties.Count!=0)
            {
                Query.Where(p => p.Properties.Any(a =>properties.Any(z=>z==a.PropertyName)));
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
