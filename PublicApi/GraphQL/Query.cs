using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using HotChocolate;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PublicApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.GraphQL
{
    public class Query
    {
        private IAsyncRepository<Product> _itemRepository;

        public Query(IAsyncRepository<Product> itemRepository)
        {
            _itemRepository = itemRepository;
        }
        [UseMagShopContext]
        public async Task<List<User>> GetUsers([ScopedService] MagShopContext context) =>
            await context.Users.ToListAsync();

        [UseMagShopContext]
        public async Task<List<Store>> GetStores([ScopedService] MagShopContext context) =>
            await context.Stores.ToListAsync();

        [UseMagShopContext]
        public async Task<List<Order>> GetOrders([ScopedService] MagShopContext context) =>
            await context.Orders.ToListAsync();

        [UseMagShopContext]
        public async Task<IReadOnlyList<Product>> GetProducts([ScopedService] MagShopContext context) =>
            await _itemRepository.ListAllAsync();
    }
}
