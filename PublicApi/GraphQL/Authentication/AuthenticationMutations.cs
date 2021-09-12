using ApplicationCore.Interfaces;
using ApplicationCore.RESTApi.Authentication;
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
    [ExtendObjectType(typeof(Mutation))]
    public class AuthenticationMutations
    {
        public async Task<AuthenticationResponse> AuthenticateByLoginPassword(AuthenticationRequest request, 
            [Service] SignInManager<UserAuthAccess> signInManager,
            [Service] IUserAuthService tokenClaimsService)
        {
            var response = new AuthenticationResponse(request.CorrelationId());

            var result = await signInManager.PasswordSignInAsync(request.UserName, request.Password, false, true);

            response.Result = result.Succeeded;
            if (!result.Succeeded)
                return null;
            response.IsLockedOut = result.IsLockedOut;
            response.IsNotAllowed = result.IsNotAllowed;
            response.RequiresTwoFactor = result.RequiresTwoFactor;
            response.Username = request.UserName;
            response.Token = await tokenClaimsService.GetTokenAsync(request.UserName);
            return response;
        }
    }
}
