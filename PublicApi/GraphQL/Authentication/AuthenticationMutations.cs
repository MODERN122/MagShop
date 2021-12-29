using ApplicationCore.GraphQLEndpoints;
using ApplicationCore.Interfaces;
using ApplicationCore.RESTApi.Authentication;
using ApplicationCore.Specifications;
using Ardalis.ApiEndpoints;
using HotChocolate;
using HotChocolate.Types;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApi.GraphQL.Authentication
{
    [ExtendObjectType(typeof(Query))]
    public class AuthenticationMutations
    {
        private IUserRepository _userRepository;

        public AuthenticationMutations(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserPayload> AuthenticateByLoginPassword(AuthenticationRequest request, 
            [Service] SignInManager<UserAuthAccess> signInManager,
            [Service] IUserAuthService tokenClaimsService)
        {
            var result = await signInManager.PasswordSignInAsync(request.UserName, request.Password, false, true);
            if (!result.Succeeded)
                throw new MemberAccessException("Пользователь не найден");
            var token = await tokenClaimsService.GetTokenAsync(request.UserName);
            var user = await _userRepository.FirstOrDefaultAsync(new UserSpecification(request.UserName));
            return new UserPayload(user, token);
        }
    }
}
