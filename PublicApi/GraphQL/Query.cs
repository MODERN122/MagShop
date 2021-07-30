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
        private IAsyncRepository<Product> _productsRepository;
        private IOrderRepository _ordersRepository;
        private IAsyncRepository<User> _usersRepository;
        private IAsyncRepository<Store> _storesRepository;

        public Query(IAsyncRepository<Product> productsRepository,
            IOrderRepository ordersRepository,
            IAsyncRepository<User> usersRepository,
            IAsyncRepository<Store> storesRepository)
        {
            _ordersRepository = ordersRepository;
            _usersRepository = usersRepository;
            _storesRepository = storesRepository;
            _productsRepository = productsRepository;
        }

        public async Task<IReadOnlyList<User>> GetUsers() =>
            await _usersRepository.ListAllAsync();

        public async Task<IReadOnlyList<Store>> GetStores() =>
            await _storesRepository.ListAllAsync();

        public async Task<Order> GetOrder(string id) =>
            await _ordersRepository.GetByIdWithItemsAsync(id);
        public async Task<IReadOnlyList<Order>> GetOrders() =>
            await _ordersRepository.ListAllAsync();

        public async Task<IReadOnlyList<Product>> GetProducts() =>
            //await context.Set<Product>().Include(x=>x.Store).Include(x=>x.Images).ToListAsync();
            await _productsRepository.ListAllAsync();
    }
}
