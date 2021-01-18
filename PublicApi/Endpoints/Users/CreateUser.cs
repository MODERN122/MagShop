using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Infrastructure.Constants;
using Infrastructure.Identity;
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

namespace ApplicationCore.Endpoints.Users
{
    public class CreateUser : BaseAsyncEndpoint<CreateUserRequest, CreateUserResponse>
    {
        private readonly IAsyncRepository<User> _userRepository;
        private readonly UserManager<UserAuthAccess> _userManager;
        private readonly IMapper _mapper;

        public CreateUser(IAsyncRepository<User> userRepository, UserManager<UserAuthAccess> userManager,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("api/users")]
        [SwaggerOperation(
            Summary = "Creates a new User",
            Description = "Creates a new User",
            OperationId = "users.create",
            Tags = new[] { "UsersEndpoints" })
        ]
        //TODO add Photos in constructor
        public override async Task<ActionResult<CreateUserResponse>> HandleAsync(CreateUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new CreateUserResponse(request.CorrelationId());

                var userAuth = new UserAuthAccess(request.Email);
                var identityResult = await _userManager.CreateAsync(userAuth, request.Password);
                if (identityResult.Succeeded)
                {
                    var user = _mapper.Map<User>(request);
                    user.Id = userAuth.Id;
                    var addedUser = await _userRepository.AddAsync(user, cancellationToken);
                    _mapper.Map(addedUser, response);
                    return Created(this.Url.ToString()+"/"+addedUser.Id, response);
                }
                else
                {
                    response.Errors = identityResult.Errors;
                    return BadRequest(response.Errors);
                }
            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }

    }
}
