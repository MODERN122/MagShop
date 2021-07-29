using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using HotChocolate;
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

        [UseMagShopContext]
        public async Task<AddProductPayload> AddProductAsync(
               AddProductInput input,
               [ScopedService] MagShopContext context)
        {
            var product = new Product
            {
                ProductName = input.ProductName,
                StoreId = input.StoreId,
                CategoryId = input.CategoryId
            };

            var res = await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            return new AddProductPayload(res?.Entity);
        }
    }
}
