using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PublicApi.GraphQL.Stores
{
    [ExtendObjectType(typeof(Mutation))]
    public class StoreMutations
    {

        private IAsyncRepository<Store> _storeRepository;

        public StoreMutations(IAsyncRepository<Store> storeRepository)
        {
            _storeRepository = storeRepository;
        }

        [Authorize(Roles = new[] { Infrastructure.Constants.ConstantsAPI.SELLERS })]
        public record RegisterStoreBySellerInput(
            string Name,
    [GraphQLType(typeof(UploadType))]
            List<IFile> ApproveDocument);
        public async Task<Store> RegisterStoreBySeller(RegisterStoreBySellerInput input,
            [Service] UserManager<UserAuthAccess> userManager,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal currentUser)
        {
            UserAuthAccess userAuth = await userManager.FindByNameAsync(currentUser.Claims.First().Value);
            //TODO Adding to S3 store
            var store = new Store(userAuth.Id, input.Name, "fake.txt");
            return await _storeRepository.AddAsync(store);
        }

    }
}
