using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Ardalis.ApiEndpoints;
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

namespace PublicApi.Endpoints.Products
{
    [Authorize(Roles = Infrastructure.Constants.ConstantsAPI.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CreateProduct : BaseAsyncEndpoint<CreateProductRequest, CreateProductResponse>
    {
        private readonly IAsyncRepository<Product> _itemRepository;
        private readonly IAsyncRepository<Store> _storeRepository;
        private readonly IUriComposer _uriComposer;
        private readonly IFileSystem _webFileSystem;
        private readonly UserManager<UserAuthAccess> _userManager;

        public CreateProduct(IAsyncRepository<Product> itemRepository, IAsyncRepository<Store> storeRepository, IUriComposer uriComposer, IFileSystem webFileSystem, UserManager<UserAuthAccess> userManager)
        {
            _itemRepository = itemRepository;
            _storeRepository = storeRepository;
            _uriComposer = uriComposer;
            _webFileSystem = webFileSystem;
            _userManager = userManager;
        }

        [HttpPost("api/products")]
        [SwaggerOperation(
            Summary = "Creates a new Product",
            Description = "Creates a new Product",
            OperationId = "products.create",
            Tags = new[] { "ProductsEndpoints" })
        ]
        //TODO add Photos in constructor
        public override async Task<ActionResult<CreateProductResponse>> HandleAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = new CreateProductResponse(request.CorrelationId());

                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var currentUserName = currentUser.FindFirst(ClaimTypes.Name).Value;
                UserAuthAccess user = await _userManager.FindByNameAsync(currentUserName);
                var store = await _storeRepository.GetByIdAsync(request.StoreId);

                if ((store != null && store.SellerId == user.Id)||currentUser.IsInRole(Infrastructure.Constants.ConstantsAPI.ADMINISTRATORS))
                {
                    var newItem = new Product(request.ProductName, request.Price, request.CategoryId, request.Description, request.Properties, request.StoreId);

                    newItem = await _itemRepository.AddAsync(newItem);
                    var productSpec = new ProductSpecification(newItem.ProductId);
                    newItem = await _itemRepository.FirstAsync(productSpec);
                    if (newItem.ProductId != null)
                    {
                        //var picName = $"{newItem}/{Path.GetExtension(request.PictureName)}";
                        //if (await _webFileSystem.SavePicture(picName, request.PictureBase64))
                        //{
                        //    newItem.UpdatePictureUri(picName);
                        //    await _itemRepository.UpdateAsync(newItem);
                        //}
                    }

                    response.Product = newItem;
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
