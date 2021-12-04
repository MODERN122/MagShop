using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
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

namespace ApplicationCore.RESTApi.Baskets
{
    [Authorize(Roles = ConstantsAPI.USERS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GetBasket : BaseAsyncEndpoint.WithRequest<string>.WithResponse<GetBasketResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<UserAuthAccess> _userManager;
        private readonly IMapper _mapper;

        public GetBasket(IUserRepository userRepository, UserManager<UserAuthAccess> userManager, IMapper mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _mapper = mapper;
        }
        [HttpGet(EndpointUrlConstants.BASKET_URL)]
        [SwaggerOperation(
               Summary = "Get a Basket by userId",
               Description = "Get a Basket by userId",
               OperationId = "baskets.get",
               Tags = new[] { "BasketsEndpoints" })
           ]
        public override async Task<ActionResult<GetBasketResponse>> HandleAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var response = new GetBasketResponse(Guid.NewGuid());
                var basket = await _userRepository.GetBasketAsync(id);
                Guard.Against.Null(basket, nameof(basket));
                response.BasketId = basket.Id;
                response.BasketItems = basket.Items
                    .Select(x => new BasketItemResponse
                    {
                        Product = x.Product,
                        Quantity = x.Quantity,
                    })
                    .ToList();
                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex);
            }
        }
    }
}
