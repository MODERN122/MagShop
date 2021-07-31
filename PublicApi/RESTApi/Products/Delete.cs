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

namespace ApplicationCore.RESTApi.Products
{
    [Authorize(Roles = "Administrators,Sellers", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Delete : BaseAsyncEndpoint.WithRequest<string>.WithResponse<DeleteUserResponse>
    {

        private readonly IAsyncRepository<Product> _productRepository;
        private readonly IAsyncRepository<Store> _storeRepository;
        private readonly UserManager<UserAuthAccess> _userManager;
        private readonly IMapper _mapper;

        public Delete(IAsyncRepository<Product> productRepository,
            IAsyncRepository<Store> storeRepository,
            UserManager<UserAuthAccess> userManager,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _storeRepository = storeRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpDelete("api/products/{id}")]
        [SwaggerOperation(
            Summary = "Deletes a new Product",
            Description = "Deletes a new Product",
            OperationId = "products.delete",
            Tags = new[] { "ProductsEndpoints" })
        ]
        public override async Task<ActionResult<DeleteUserResponse>> HandleAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                var currentUserName = currentUser.FindFirst(ClaimTypes.Name).Value;
                UserAuthAccess user = await _userManager.FindByNameAsync(currentUserName);
                var product = await _productRepository.GetByIdAsync(id);
                Guard.Against.Null(product, nameof(product));
                var store = await _storeRepository.GetByIdAsync(product.StoreId);

                if ((store != null && store.SellerId == user.Id) || currentUser.IsInRole(ConstantsAPI.ADMINISTRATORS))
                {
                    await _productRepository.DeleteAsync(product, cancellationToken);
                    return Ok(new DeleteUserResponse { Id = id });
                }
                else
                    return Forbid();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
