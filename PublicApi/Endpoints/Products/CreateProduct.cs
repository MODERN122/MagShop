using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApi.Endpoints.Products
{
    [Authorize(Roles = Infrastructure.Constants.ConstantsAPI.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CreateProduct : BaseAsyncEndpoint<CreateProductRequest, CreateProductResponse>
    {
        private readonly IAsyncRepository<Product> _itemRepository;
        private readonly IUriComposer _uriComposer;
        private readonly IFileSystem _webFileSystem;

        public CreateProduct(IAsyncRepository<Product> itemRepository, IUriComposer uriComposer, IFileSystem webFileSystem)
        {
            _itemRepository = itemRepository;
            _uriComposer = uriComposer;
            _webFileSystem = webFileSystem;
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
            var response = new CreateProductResponse(request.CorrelationId());

            var newItem = new Product(request.ProductName, request.Price, request.CategoryId, request.Description, request.Properties, request.StoreId);

            newItem = await _itemRepository.AddAsync(newItem);

            if (newItem.ProductId != null)
            {
                //var picName = $"{newItem}/{Path.GetExtension(request.PictureName)}";
                //if (await _webFileSystem.SavePicture(picName, request.PictureBase64))
                //{
                //    newItem.UpdatePictureUri(picName);
                //    await _itemRepository.UpdateAsync(newItem);
                //}
            }

            //var dto = new CatalogItemDto
            //{
            //    Id = newItem.Id,
            //    CatalogBrandId = newItem.CatalogBrandId,
            //    CatalogTypeId = newItem.CatalogTypeId,
            //    Description = newItem.Description,
            //    Name = newItem.Name,
            //    PictureUri = _uriComposer.ComposePicUri(newItem.PictureUri),
            //    Price = newItem.Price
            //};
            response.Product = newItem;
            return response;
        }
    }
}
