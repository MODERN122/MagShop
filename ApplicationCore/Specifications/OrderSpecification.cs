using ApplicationCore.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(int pageIndex = 0, int pageSize = 20) : base(pageIndex, pageSize)
        {
            Query
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.Images);
            Query.Include(x => x.ShipToAddress);
        }
    }
}
