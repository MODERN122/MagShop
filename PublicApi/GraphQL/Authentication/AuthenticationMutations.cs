using ApplicationCore.GraphQLEndpoints;
using ApplicationCore.Interfaces;
using ApplicationCore.RESTApi.Authentication;
using ApplicationCore.Specifications;
using HotChocolate;
using HotChocolate.Types;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace PublicApi.GraphQL.Authentication
{
    [ExtendObjectType(typeof(Mutation))]
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

        [AllowAnonymous]
        public async Task<Token> RefreshTokenAsync(Token token,
            [Service] IUserAuthService tokenClaimsService)
        {
            var newToken = await tokenClaimsService.RefreshTokenAsync(token.RefreshToken);
            return newToken;
        }
    }
}
