using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public async Task<IReadOnlyList<Product>> GetProducts() =>
            await _productRepository.ListAllAsync();
    }
}
