using ApplicationCore.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class OrderSpecification : BaseSpecification<Order>
    {        
        public OrderSpecification(string id = null, string userId = null, int pageIndex = 0, int pageSize = 20) : base(pageIndex, pageSize)
        {
            if(id != null)
            {
                Query.Where(x => x.Id == id);
            }

            if(userId != null)
            {
                Query.Where(x=>x.UserId==userId);
            }

            Query
                .Include(x => x.User);
            Query.Include(x => x.ShipToAddress);


            Query
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductProperties)
                .ThenInclude(x => x.ProductPropertyItems);

            //Query
            //    .Include(x => x.Items)
            //    .ThenInclude(x => x.Product)
            //    .ThenInclude(x => x.Category);
            //Query.Include(x => x.Basket)
            //    .ThenInclude(x => x.Items)
            //    .ThenInclude(x => x.Product)
            //    .ThenInclude(x => x.Images);

            Query
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .ThenInclude(p => p.Properties);

            //Query
            //    .Include(x => x.Items)
            //    .ThenInclude(x => x.Product)
            //    .ThenInclude(p => p.Store);

        }
    }
}
