using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.Endpoints.Users
{
    using ApplicationCore.Endpoints.Users;
    using ApplicationCore.Entities;
    using ApplicationCore.Interfaces;
    using Ardalis.ApiEndpoints;
    using Ardalis.GuardClauses;
    using AutoMapper;
    using Infrastructure.Constants;
    using Infrastructure.Identity;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;

    namespace PublicApi.Endpoints.Users
    {
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public class GetUser : BaseAsyncEndpoint.WithoutRequest.WithResponse<GetUserResponse>
        {
            private readonly IAsyncRepository<User> _userRepository;
            private readonly UserManager<UserAuthAccess> _userManager;
            private readonly ITokenClaimsService _tokenClaimsService;
            private readonly IMapper _mapper;

            public GetUser(IAsyncRepository<User> userRepository, UserManager<UserAuthAccess> userManager,
                IMapper mapper, ITokenClaimsService tokenClaimsService)
            {
                _userRepository = userRepository;
                _userManager = userManager;
                _mapper = mapper;
                _tokenClaimsService = tokenClaimsService;
            }

            [HttpGet("api/users")]
            [SwaggerOperation(
                Summary = "Get a User",
                Description = "Get a User",
                OperationId = "user.get",
                Tags = new[] { "UsersEndpoints" })
            ]
            public override async Task<ActionResult<GetUserResponse>> HandleAsync(CancellationToken cancellationToken = default)
            {
                try
                {
                    ClaimsPrincipal currentUser = this.User;
                    var currentUserName = currentUser.FindFirst(ClaimTypes.Name).Value;
                    UserAuthAccess userAuth = await _userManager.FindByNameAsync(currentUserName);
                    var response = new GetUserResponse(Guid.NewGuid());
                    var user = await _userRepository.GetByIdAsync(userAuth.Id);
                    Guard.Against.Null(user, nameof(user));
                    response.User = user;
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }
        }
    }

}
