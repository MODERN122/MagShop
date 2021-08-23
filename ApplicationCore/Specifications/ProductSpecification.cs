using ApplicationCore.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(int pageIndex = 0, int pageSize = 20) : base(pageIndex, pageSize) { }
        public ProductSpecification(string productId) : base(0, 1)
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
        public ProductSpecification(string categoryId, string storeId, List<string> properties, int pageIndex = 0, int pageSize = 20) : base(pageIndex, pageSize)
        {
            Query
                .Where(p => (string.IsNullOrWhiteSpace(categoryId) || p.CategoryId == categoryId) && (string.IsNullOrWhiteSpace(storeId) || p.StoreId == storeId));
            if (properties != null && properties.Count != 0)
            {
                Query.Where(p => p.Properties.Any(a => properties.Any(z => z == a.PropertyName)));
            }
            Query
                .Include(i => i.Category);
            Query.Include(p => p.Properties)
                .ThenInclude(i => i.PropertyItems);
        }
    }
}
