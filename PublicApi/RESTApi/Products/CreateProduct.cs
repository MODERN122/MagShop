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

namespace ApplicationCore.RESTApi.Products
{
    [Authorize(Roles = ConstantsAPI.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize(Roles = ConstantsAPI.SELLERS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CreateProduct : BaseAsyncEndpoint.WithRequest<CreateProductRequest>.WithResponse<CreateProductResponse>
    {
        private readonly IAsyncRepository<Product> _itemRepository;
        private readonly IAsyncRepository<Store> _storeRepository;
        private readonly UserManager<UserAuthAccess> _userManager;
        private readonly IMapper _mapper;

        public CreateProduct(IAsyncRepository<Product> itemRepository, 
            IAsyncRepository<Store> storeRepository, 
            UserManager<UserAuthAccess> userManager,
            IMapper mapper)
        {
            _itemRepository = itemRepository;
            _storeRepository = storeRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("api/products")]
        [SwaggerOperation(
            Summary = "Creates a new Product",
            Description = "Creates a new Product",
            OperationId = "products.create",
            Tags = new[] { "ProductsEndpoints" })
        ]
        //TODO add Photos in constructor
        public override async Task<ActionResult<CreateProductResponse>> HandleAsync(CreateProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new CreateProductResponse(request.CorrelationId());

                ClaimsPrincipal currentUser = this.User;
                var currentUserName = currentUser.FindFirst(ClaimTypes.Name).Value;
                UserAuthAccess user = await _userManager.FindByNameAsync(currentUserName);
                var store = await _storeRepository.GetByIdAsync(request.StoreId);

                if ((store != null && store.SellerId == user.Id)||currentUser.IsInRole(ConstantsAPI.ADMINISTRATORS))
                {
                    var product = new Product();
                    _mapper.Map(request, product);
                    product = await _itemRepository.AddAsync(product, cancellationToken);
                    if (product.Id != null)
                    {
                        //var picName = $"{newItem}/{Path.GetExtension(request.PictureName)}";
                        //if (await _webFileSystem.SavePicture(picName, request.PictureBase64))
                        //{
                        //    newItem.UpdatePictureUri(picName);
                        //    await _itemRepository.UpdateAsync(newItem);
                        //}
                    }

                    response.Product = product;
                    return Created(this.Url.ToString() + "/" + product.Id, response);
                }
                else
                {
                    return Forbid();
                }
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
