using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.RESTApi.Authentication
{
    public class Authenticate : BaseAsyncEndpoint
        .WithRequest<AuthenticationRequest>
        .WithResponse<AuthenticationResponse>
    {
        private readonly SignInManager<UserAuthAccess> _signInManager;
        private readonly IUserAuthService _tokenClaimsService;

        public Authenticate(SignInManager<UserAuthAccess> signInManager,
            IUserAuthService tokenClaimsService)
        {
            _signInManager = signInManager;
            _tokenClaimsService = tokenClaimsService;
        }
        
        [HttpPost(EndpointUrlConstants.AUTHENTICATION_URL)]
        [SwaggerOperation(
            Summary = "Authenticates a user",
            Description = "Authenticates a user",
            OperationId = "auth.authenticate",
            Tags = new[] { "AuthEndpoints" })
        ]
        public override async Task<ActionResult<AuthenticationResponse>> HandleAsync(AuthenticationRequest request, CancellationToken cancellationToken)
        {
            var response = new AuthenticationResponse(request.CorrelationId());

            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, true);

            response.Result = result.Succeeded;
            if (!result.Succeeded)
                return NotFound();
            response.IsLockedOut = result.IsLockedOut;
            response.IsNotAllowed = result.IsNotAllowed;
            response.RequiresTwoFactor = result.RequiresTwoFactor;
            response.Username = request.UserName;
            response.Token = await _tokenClaimsService.GetTokenAsync(request.UserName);

            return Ok(response);
        }

    }
}
