using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using HotChocolate.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.GraphQL.Products
{
    public class ProductMutaions
    {
        private IAsyncRepository<Product> _productRepository;

        public ProductMutaions(
            IAsyncRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }
        public record AddProductInput(
            string ProductName,
            string StoreId,
            string CategoryId,
            List<string> PropertiesId);
        public class AddProductPayload
        {
            public AddProductPayload(Product product)
            {
                Product = product;
            }

            public Product Product { get; }
        }

        [Authorize]
        public async Task<AddProductPayload> AddProductAsync(
               AddProductInput input)
        {
            var product = new Product
            {
                Name = input.ProductName,
                StoreId = input.StoreId,
                CategoryId = input.CategoryId
            };
            var result = await _productRepository.AddAsync(product, new System.Threading.CancellationToken());
            return new AddProductPayload(result);
        }
    }
}
