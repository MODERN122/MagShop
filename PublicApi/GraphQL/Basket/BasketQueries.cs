using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
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

namespace PublicApi.GraphQL.Basket
{
    [ExtendObjectType(typeof(Query))]
    public class BasketQueries
    {
        private readonly IUserRepository _userRepository;

        public BasketQueries(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize(Roles = new[] { Infrastructure.Constants.ConstantsAPI.USERS, Infrastructure.Constants.ConstantsAPI.ADMINISTRATORS })]
        public async Task<ApplicationCore.Entities.Basket> GetBasket(
            [Service] UserManager<UserAuthAccess> userManager,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal currentUser)
        {
            return await _userRepository.GetBasketAsync(currentUser.Claims.First().Value);
        }

    }
}
