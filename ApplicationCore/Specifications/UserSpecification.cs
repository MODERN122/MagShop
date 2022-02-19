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
            Query.Where(x => x.Id == userId)
                .Include(x=>x.Basket)
                .ThenInclude(x => x.Items)
                .ThenInclude(x => x.Product)
                .ThenInclude(x=>x.Category);
        }
    }
}
