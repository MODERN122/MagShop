using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
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
        private IAsyncRepository<User> _usersRepository;
        private IAsyncRepository<Store> _storesRepository;

        public Query(IAsyncRepository<User> usersRepository,
            IAsyncRepository<Store> storesRepository)
        {
            _usersRepository = usersRepository;
            _storesRepository = storesRepository;
        }

        [Authorize(Roles = new[] { "Administrators" })]
        public async Task<IEnumerable<User>> GetUsers() =>
            await _usersRepository.ListAllAsync();

        public async Task<IEnumerable<Store>> GetStores() =>
            await _storesRepository.ListAllAsync();


    }
}
