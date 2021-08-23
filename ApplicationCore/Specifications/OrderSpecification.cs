using ApplicationCore.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class OrderSpecification : Specification<Order>
    {
        public OrderSpecification(int skip = 0, int limit = int.MaxValue)
        {
            Query
                .Skip(skip)
                .Take(limit);
            Query
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.Images);
            Query.Include(x => x.ShipToAddress);
        }
    }
}
