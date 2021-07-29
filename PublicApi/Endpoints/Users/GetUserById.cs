using ApplicationCore.Endpoints.Users;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using AutoMapper;
using Infrastructure.Constants;
using Infrastructure.Identity;
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
    public class GetUserById : BaseAsyncEndpoint.WithRequest<string>.WithResponse<GetUserResponse>
    {
        private readonly IAsyncRepository<User> _userRepository;
        private readonly UserManager<UserAuthAccess> _userManager;
        private readonly ITokenClaimsService _tokenClaimsService;
        private readonly IMapper _mapper;

        public GetUserById(IAsyncRepository<User> userRepository, UserManager<UserAuthAccess> userManager,
            IMapper mapper, ITokenClaimsService tokenClaimsService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _mapper = mapper;
            _tokenClaimsService = tokenClaimsService;
        }

        [HttpGet("api/users/{id}")]
        [SwaggerOperation(
            Summary = "Get a User by id",
            Description = "Get a User by id",
            OperationId = "userId.get",
            Tags = new[] { "UsersEndpoints" })
        ]
        public override async Task<ActionResult<GetUserResponse>> HandleAsync(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                ClaimsPrincipal currentUser = this.User;
                var currentUserName = currentUser.FindFirst(ClaimTypes.Name).Value;
                UserAuthAccess userAuth = await _userManager.FindByNameAsync(currentUserName);
                if (this.User.IsInRole(ConstantsAPI.ADMINISTRATORS) || userAuth.Id == id)
                {
                    var response = new GetUserResponse(Guid.NewGuid());
                    var user = await _userRepository.GetByIdAsync(id);
                    Guard.Against.Null(user, nameof(user));
                    response.User = user;
                    return Ok(response);
                }
                else
                {
                    return Forbid("У вас недостаточно прав, чтобы просмотреть данные этого пользователя");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
