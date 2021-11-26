using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.RESTApi.Users;
using Ardalis.GuardClauses;
using AutoMapper;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Infrastructure.Constants;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PublicApi.GraphQL.Users
{
    [ExtendObjectType(typeof(Query))]
    public class UserQueries
    {
        private readonly IAsyncRepository<User> _usersRepository;

        public UserQueries(IAsyncRepository<User> usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [Authorize]
        public async Task<User> GetUser(
            [Service] IAsyncRepository<User> userRepository,
            [Service] IHttpContextAccessor contextAccessor,
            [Service] UserManager<UserAuthAccess> userManager,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal currentUser)
        {
            UserAuthAccess userAuth = await userManager.FindByNameAsync(currentUser.Claims.First().Value);
            if (userAuth != null)
            {
                var user = await userRepository.GetByIdAsync(userAuth.Id);
                return user;
            }
            return null;
        }

        [Authorize(Roles = new[] { ConstantsAPI.ADMINISTRATORS})]
        public async Task<IEnumerable<User>> GetUsers() =>
            await _usersRepository.ListAllAsync();

        [Authorize(Roles = new string[] { ConstantsAPI.ADMINISTRATORS })]
        public async Task<User> GetUserById(
            string id,
            [Service] IAsyncRepository<User> userRepository,
            [Service] UserManager<UserAuthAccess> userManager,
            [Service] IHttpContextAccessor contextAccessor,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal currentUser)
        {
            //UserAuthAccess userAuth = await userManager.FindByNameAsync(currentUser.Claims.First().Value);
            var user = await userRepository.GetByIdAsync(id);
            return user;
        }

    }
}
