using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PublicApi.GraphQL.Stores
{
    [ExtendObjectType(typeof(Query))]
    public class StoreQueries
    {
        private readonly IAsyncRepository<Store> _storesRepository;

        public StoreQueries(
            IAsyncRepository<Store> storesRepository)
        {

            _storesRepository = storesRepository;
        }

        [Authorize(Roles = new[] { ConstantsAPI.SELLERS })]
        public async Task<IEnumerable<Store>> GetStores(string sellerId, int pageIndex = 0, int pageSize = 20,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal currentUser = default) =>
            currentUser.Claims.First().Value == sellerId 
            ? await _storesRepository.ListAsync(new StoreSpecification(sellerId, pageIndex, pageSize)) 
            : throw new Exception("Нет доступа");
    }
}
