﻿using ApplicationCore.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(int pageIndex, int pageSize) : base(pageIndex, pageSize) { }

        public ProductSpecification(string productId, bool isBaseEntity = false) : base(0, 1)
        {
            Query
                .Where(p => p.Id == productId);
            if (!isBaseEntity)
            {
                Query
                    .Include(i => i.Category);
                Query.Include(x => x.ProductProperties).ThenInclude(x => x.ProductPropertyItems);
                Query.Include(x => x.ChoosenProducts).ThenInclude(x => x.PropertyItemTuples);
                Query.Include(p => p.Properties);
                Query.
                    Include(p => p.Images);
                Query.Include(p => p.Store);
            }
        }

        public ProductSpecification(string[] ids) : base(0, ids.Length)
        {
            Query.Where(x => ids.Contains(x.Id));
            Query
                .Include(i => i.Category);

            Query.Include(x => x.ProductProperties).ThenInclude(x => x.Property).ThenInclude(x => x.Items);
        }

        public ProductSpecification(string categoryId, string storeId, List<string> propertiesId, int pageIndex, int pageSize, int? minDiscount = null) : base(pageIndex, pageSize)
        {
            Query
                .Where(p => (string.IsNullOrWhiteSpace(categoryId) || p.CategoryId == categoryId) && (string.IsNullOrWhiteSpace(storeId) || p.StoreId == storeId));
            if (propertiesId != null && propertiesId.Count != 0)
            {
                Query.Where(p => p.Properties.Any(a => propertiesId.Any(z => z == a.Id)));
            }
            if (minDiscount != null)
            {
                Query.Where(p => p.Discount > minDiscount.Value);
            }
            Query
                .Include(i => i.Category);

            Query.Include(x => x.ProductProperties).ThenInclude(x => x.Property).ThenInclude(x => x.Items);
        }
    }
}
