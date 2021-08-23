using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PublicApi.GraphQL.Products
{
    [ExtendObjectType(typeof(Query))]
    public class ProductQueries
    {
        private IAsyncRepository<Product> _productRepository;
        public ProductQueries(IAsyncRepository<Product> productRepository)
        {

            _productRepository = productRepository;
        }

        public async Task<IReadOnlyList<Product>> GetProductsSpec(int pageIndex = 0, int pageSize = 20)
        {
            var spec = new ProductSpecification(pageIndex, pageSize);
            return await _productRepository.ListAsync(spec);
        }

        public async Task<IReadOnlyList<Product>> GetProducts() =>
            await _productRepository.ListAllAsync();
    }
}
