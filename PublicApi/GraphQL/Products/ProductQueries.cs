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

        public async Task<IReadOnlyList<Product>> GetSearchProducts(int pageIndex = 0, int pageSize = 20, string categoryId = null, string storeId = null,
            List<string> propertiesId = null)
        {
            var spec = new ProductSpecification(categoryId: categoryId, storeId: storeId, properties: propertiesId, pageIndex, pageSize);
            return await _productRepository.ListAsync(spec);
        }

        public async Task<IReadOnlyList<Product>> GetProducts() =>
            await _productRepository.ListAllAsync();
    }
}
