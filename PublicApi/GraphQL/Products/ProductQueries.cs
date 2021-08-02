using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
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

        [Authorize(Roles =new[] { "Administrators" })]
        public async Task<IReadOnlyList<Product>> GetProducts(
            [Service] IHttpContextAccessor contextAccessor) =>
            await _productRepository.ListAllAsync();
    }
}
