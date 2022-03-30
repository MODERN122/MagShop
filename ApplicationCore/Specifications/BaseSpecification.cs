using ApplicationCore.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class BaseSpecification<T> : Specification<T>
    {
        public BaseSpecification(int pageIndex, int pageSize)
        {
            Query
                .Skip(pageSize * pageIndex)
                .Take(pageSize);
        }
    }
}
