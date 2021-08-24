using ApplicationCore.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class PropertySpecification: Specification<Property>
    {
        public PropertySpecification()
        {
            Query.Include(x => x.Items);
        }
    }
}
