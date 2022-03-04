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
using System.IO;
using PublicApi.Providers.AwsS3;

namespace PublicApi.RESTApi.Media
{
    public class CreateImageProduct : BaseAsyncEndpoint.WithRequest<CreateImageProductRequest>.WithResponse<CreateImageProductResponse>
    {
        private readonly IAsyncRepository<Product> _itemRepository;
        private readonly IAsyncRepository<ProductImage> _imageRepository;
        private readonly UserManager<UserAuthAccess> _userManager;
        private readonly IMapper _mapper;

        public CreateImageProduct(IAsyncRepository<Product> itemRepository,
            IAsyncRepository<ProductImage> imageRepository,
            UserManager<UserAuthAccess> userManager,
            IMapper mapper)
        {
            _itemRepository = itemRepository;
            _imageRepository = imageRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("api/media/image")]
        [SwaggerOperation(
            Summary = "Creates a new Image Product",
            Description = "Creates a new Image Product",
            OperationId = "imageProduct.create",
            Tags = new[] { "ImageProductsEndpoints" })
        ]
        public override async Task<ActionResult<CreateImageProductResponse>> HandleAsync([FromForm] CreateImageProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using Stream stream = request.File.OpenReadStream();

                if (stream.Length == 0)
                {
                    return BadRequest("Изображение пустое");
                }
                var product = await _itemRepository.FirstOrDefaultAsync(new ProductSpecification(request.ProductId, true));
                if (product == null)
                {
                    return BadRequest("Товар не найден");
                }
                var result = await Provider.UploadFromStream(stream, request.ImageName.Split('.').Last(), $"image/{product.StoreId}/{product.Id}");
                var image = new ProductImage(request.ProductId, result);
                var res = await _imageRepository.AddAsync(image, cancellationToken);
                if (res!=null)
                {
                    return Ok(new CreateImageProductResponse(Guid.NewGuid()) { ImagePath = res.Path });
                }
                else
                {
                    return BadRequest("Ошибка добавления изображения товара");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
