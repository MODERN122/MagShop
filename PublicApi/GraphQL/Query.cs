using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using HotChocolate;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
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
        private IAsyncRepository<User> _usersRepository;
        private IAsyncRepository<Store> _storesRepository;

        public Query(IAsyncRepository<Product> productsRepository,
            IAsyncRepository<User> usersRepository,
            IAsyncRepository<Store> storesRepository)
        {
            _usersRepository = usersRepository;
            _storesRepository = storesRepository;
            _productsRepository = productsRepository;
        }

        public async Task<IEnumerable<User>> GetUsers() =>
            await _usersRepository.ListAllAsync();

        public async Task<IEnumerable<Store>> GetStores() =>
            (await _storesRepository.ListAllAsync());


    }
}
