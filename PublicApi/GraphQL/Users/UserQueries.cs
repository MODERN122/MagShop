using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.RESTApi.Users;
using Ardalis.GuardClauses;
using AutoMapper;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
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
        [Authorize]
        public async Task<GetUserResponse> GetUser(
            [Service] IAsyncRepository<User> userRepository,
            [Service] UserManager<UserAuthAccess> userManager,
            ClaimsPrincipal currentUser)
        {
            UserAuthAccess userAuth = await userManager.GetUserAsync(currentUser);
            if (userAuth != null)
            {
                var response = new GetUserResponse(Guid.NewGuid());
                var user = await userRepository.GetByIdAsync(userAuth.Id);
                Guard.Against.Null(user, nameof(user));
                response.User = user;
                return response;
            }
            return null;
        }

        [Authorize(Roles = new string[] {"Administators"})]
        public async Task<GetUserResponse> GetUserById(
            [Service] IAsyncRepository<User> userRepository, 
            [Service] UserManager<UserAuthAccess> userManager, 
            [Service] IHttpContextAccessor contextAccessor,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal currentUser)
        {
            UserAuthAccess userAuth = await userManager.GetUserAsync(currentUser);
            if (userAuth != null)
            {
                var response = new GetUserResponse(Guid.NewGuid());
                var user = await userRepository.GetByIdAsync(userAuth.Id);
                Guard.Against.Null(user, nameof(user));
                response.User = user;
                return response;
            }
            return null;
        }

    }
}
