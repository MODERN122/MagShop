using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PublicApi.GraphQL.Basket
{
    [ExtendObjectType(typeof(Mutation))]
    public class BasketMutations
    {
        private readonly IUserRepository _userRepository;

        public BasketMutations(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize(Roles = new[] { Infrastructure.Constants.ConstantsAPI.USERS, Infrastructure.Constants.ConstantsAPI.ADMINISTRATORS })]
        public async Task<List<ApplicationCore.Entities.BasketItem>> AddBasketItem(string productId,
            [Service] UserManager<UserAuthAccess> userManager,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal currentUser)
        {
            var id = currentUser.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
           var basket = await _userRepository.AddBasketItem(id, productId);
            return basket?.Items;
        }

        [Authorize(Roles = new[] { Infrastructure.Constants.ConstantsAPI.USERS, Infrastructure.Constants.ConstantsAPI.ADMINISTRATORS })]
        public async Task<List<ApplicationCore.Entities.BasketItem>> RemoveBasketItem(string productId,
            [Service] UserManager<UserAuthAccess> userManager,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal currentUser)
        {
            var id = currentUser.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            var basket = await _userRepository.RemoveBasketItem(id, productId);
            return basket?.Items;
        }

        [Authorize(Roles = new[] { Infrastructure.Constants.ConstantsAPI.USERS, Infrastructure.Constants.ConstantsAPI.ADMINISTRATORS })]
        public async Task<List<ApplicationCore.Entities.BasketItem>> SubstractBasketItem(string productId,
            [Service] UserManager<UserAuthAccess> userManager,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal currentUser)
        {
            var id = currentUser.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            var basket = await _userRepository.SubstractBasketItem(id, productId);
            return basket?.Items;
        }
    }
}
