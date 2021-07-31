using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Ardalis.ApiEndpoints;
using AutoMapper;
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
using Infrastructure.Constants;
using Ardalis.GuardClauses;

namespace ApplicationCore.RESTApi.Products
{
    [Authorize(Roles = "Administrators,Sellers", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize(Roles = ConstantsAPI.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Update : BaseAsyncEndpoint.WithRequest<PutProductRequest>.WithResponse<PutProductResponse>
    {
        private readonly IAsyncRepository<Product> _productRepository;
        private readonly IAsyncRepository<Store> _storeRepository;
        private readonly UserManager<UserAuthAccess> _userManager;
        private readonly IMapper _mapper;

        public Update(IAsyncRepository<Product> productRepository, 
            IAsyncRepository<Store> storeRepository, 
            UserManager<UserAuthAccess> userManager,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _storeRepository = storeRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPut("api/products/{id}")]
        [SwaggerOperation(
            Summary = "Updates a new Product",
            Description = "Updates a new Product",
            OperationId = "products.update",
            Tags = new[] { "ProductsEndpoints" })
        ]
        //TODO add Photos in constructor
        public override async Task<ActionResult<PutProductResponse>> HandleAsync(PutProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new PutProductResponse(request.CorrelationId());

                ClaimsPrincipal currentUser = this.User;
                var currentUserName = currentUser.FindFirst(ClaimTypes.Name).Value;
                UserAuthAccess user = await _userManager.FindByNameAsync(currentUserName);
                var oldProduct = await _productRepository.GetByIdAsync(request.Id);
                Guard.Against.Null(oldProduct, nameof(oldProduct));
                //if (oldProduct == null)
                //{
                //    CreateProduct createProduct = new CreateProduct(_productRepository, _storeRepository, _userManager, _mapper);

                //    var s = await createProduct.HandleAsync(_mapper.Map<CreateProductRequest>(request), cancellationToken);
                //    response.Product = s.Value.Product;
                //    return response;
                //}
                var store = await _storeRepository.GetByIdAsync(oldProduct.StoreId);

                if ((store != null && store.SellerId == user.Id) || currentUser.IsInRole(Infrastructure.Constants.ConstantsAPI.ADMINISTRATORS))
                {
                    var newProd = _mapper.Map(request, oldProduct);
                    await _productRepository.UpdateAsync(oldProduct, cancellationToken);
                    var productSpec = new ProductSpecification(oldProduct.ProductId);
                    oldProduct = await _productRepository.FirstAsync(productSpec);
                    if (oldProduct.ProductId != null)
                    {

                    }

                    response.Product = oldProduct;
                    return Ok(response);
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
