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

namespace ApplicationCore.Endpoints.Baskets
{
    [Authorize(Roles = ConstantsAPI.USERS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GetBasket : BaseAsyncEndpoint<string, GetBasketResponse>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IAsyncRepository<User> _userRepository;
        private readonly UserManager<UserAuthAccess> _userManager;
        private readonly IMapper _mapper;

        public GetBasket(IBasketRepository basketRepository, UserManager<UserAuthAccess> userManager, IMapper mapper, IAsyncRepository<User> userRepository)
        {
            _basketRepository = basketRepository;
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
                var basket = await _basketRepository.FirstAsync(id, new BasketSpecification(id), cancellationToken);
                Guard.Against.Null(basket, nameof(basket));
                response.BasketId = basket.BasketId;
                response.BasketItems = basket.Items
                    .Select(x => new BasketItemResponse
                    {
                        Product = x.Product,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice.Value
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
