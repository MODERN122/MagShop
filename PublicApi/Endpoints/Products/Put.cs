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

namespace PublicApi.Endpoints.Products
{
    [Authorize(Roles = "Administrators,Sellers", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize(Roles = ConstantsAPI.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Put : BaseAsyncEndpoint<PutProductRequest, PutProductResponse>
    {
        private readonly IAsyncRepository<Product> _productRepository;
        private readonly IAsyncRepository<Store> _storeRepository;
        private readonly UserManager<UserAuthAccess> _userManager;
        private readonly IMapper _mapper;

        public Put(IAsyncRepository<Product> productRepository, 
            IAsyncRepository<Store> storeRepository, 
            UserManager<UserAuthAccess> userManager,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _storeRepository = storeRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPut("api/products")]
        [SwaggerOperation(
            Summary = "Updates a new Product",
            Description = "Updates a new Product",
            OperationId = "products.update",
            Tags = new[] { "ProductsEndpoints" })
        ]
        //TODO add Photos in constructor
        public override async Task<ActionResult<PutProductResponse>> HandleAsync(PutProductRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = new PutProductResponse(request.CorrelationId());

                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var currentUserName = currentUser.FindFirst(ClaimTypes.Name).Value;
                UserAuthAccess user = await _userManager.FindByNameAsync(currentUserName);
                var oldProduct = await _productRepository.GetByIdAsync(request.ProductId);
                var store = await _storeRepository.GetByIdAsync(oldProduct.StoreId);

                if ((store != null && store.SellerId == user.Id) || currentUser.IsInRole(Infrastructure.Constants.ConstantsAPI.ADMINISTRATORS))
                {
                     var newProd = _mapper.Map(request, oldProduct);
                    await _productRepository.UpdateAsync(oldProduct);
                    var productSpec = new ProductSpecification(oldProduct.ProductId);
                    oldProduct = await _productRepository.FirstAsync(productSpec);
                    if (oldProduct.ProductId != null)
                    {
                        //var picName = $"{newItem}/{Path.GetExtension(request.PictureName)}";
                        //if (await _webFileSystem.SavePicture(picName, request.PictureBase64))
                        //{
                        //    newItem.UpdatePictureUri(picName);
                        //    await _itemRepository.UpdateAsync(newItem);
                        //}
                    }

                    response.Product = oldProduct;
                    return Ok(response);

                }
                else
                {
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }
    }
}
