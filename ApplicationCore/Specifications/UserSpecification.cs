using ApplicationCore.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class UserSpecification : Specification<User>
    {
        /// <summary>
        /// Get basket
        /// </summary>
        /// <param name="userId"></param>
        public UserSpecification(string userId)
        {
            Query.Where(x => x.Id == userId);
            Query.Include(x => x.Basket)
                .ThenInclude(x => x.Items)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductProperties)
                .ThenInclude(x => x.ProductPropertyItems);

            //Query.Include(x => x.Basket)
            //    .ThenInclude(x => x.Items)
            //    .ThenInclude(x => x.Product)
            //    .ThenInclude(x => x.Category);
            //Query.Include(x => x.Basket)
            //    .ThenInclude(x => x.Items)
            //    .ThenInclude(x => x.Product)
            //    .ThenInclude(x => x.Images);

            Query.Include(x => x.Basket)
                .ThenInclude(x => x.Items)
                .ThenInclude(x => x.Product)
                .ThenInclude(p => p.Properties);

            //Query.Include(x => x.Basket)
            //    .ThenInclude(x => x.Items)
            //    .ThenInclude(x => x.Product)
            //    .ThenInclude(p => p.Store);
        }
    }
}
