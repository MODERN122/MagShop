using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using HotChocolate;
using HotChocolate.Types;
using Infrastructure.Data;
using PublicApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.GraphQL
{
    public class Mutation
    {
        private IAsyncRepository<Product> _productRepository;

        public Mutation(
            IAsyncRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }
        public record AddProductInput(
            string ProductName,
            string StoreId,
            string CategoryId); 
        public class AddProductPayload
        {
            public AddProductPayload(Product product)
            {
                Product = product;
            }

            public Product Product { get; }
        }

        public async Task<AddProductPayload> AddProductAsync(
               AddProductInput input)
        {
            var product = new Product
            {
                ProductName = input.ProductName,
                StoreId = input.StoreId,
                CategoryId = input.CategoryId
            };
            var result = await _productRepository.AddAsync(product, new System.Threading.CancellationToken());
            return new AddProductPayload(result);
        }
    }
}
