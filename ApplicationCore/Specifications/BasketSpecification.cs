using ApplicationCore.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class BasketSpecification : Specification<Basket>
    {
        public BasketSpecification(string id)
        {
            Query.Where(x => x.UserId == id)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product);
        }
    }
}
