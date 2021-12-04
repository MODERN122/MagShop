using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class StoreSpecification:BaseSpecification<Store>
    {
        public StoreSpecification(string sellerId, int pageIndex = 0, int pageSize = 20) : base(pageIndex, pageSize)
        {
            Query.Where(x => x.SellerId == sellerId);
            Query
                .Include(x => x.StoreProducts);
            Query.Include(x => x.Seller);
        }
    }
}
