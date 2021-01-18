
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

namespace ApplicationCore.Endpoints.Authentication
{
    public class Authenticate : BaseAsyncEndpoint<AuthenticationRequest, AuthenticationResponse>
    {
        private readonly SignInManager<UserAuthAccess> _signInManager;
        private readonly ITokenClaimsService _tokenClaimsService;

        public Authenticate(SignInManager<UserAuthAccess> signInManager,
            ITokenClaimsService tokenClaimsService)
        {
            _signInManager = signInManager;
            _tokenClaimsService = tokenClaimsService;
        }

        [HttpPost("api/authentication")]
        [SwaggerOperation(
            Summary = "Authenticates a user",
            Description = "Authenticates a user",
            OperationId = "auth.authenticate",
            Tags = new[] { "AuthEndpoints" })
        ]
        public override async Task<ActionResult<AuthenticationResponse>> HandleAsync(AuthenticationRequest request, CancellationToken cancellationToken)
        {
            var response = new AuthenticationResponse(request.CorrelationId());

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);

            var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, true);

            response.Result = result.Succeeded;
            if (!result.Succeeded)
                return NotFound();
            response.IsLockedOut = result.IsLockedOut;
            response.IsNotAllowed = result.IsNotAllowed;
            response.RequiresTwoFactor = result.RequiresTwoFactor;
            response.Username = request.Username;
            response.Token = await _tokenClaimsService.GetTokenAsync(request.Username);

            return Ok(response);
        }

    }
}
