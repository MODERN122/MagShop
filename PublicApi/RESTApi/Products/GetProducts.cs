using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.RESTApi.Products
{

    public class GetProducts : BaseAsyncEndpoint.WithRequest<GetProductsRequest>.WithResponse<GetProductsResponse>
    {
        private readonly IAsyncRepository<Product> _productRepository;

        public GetProducts(IAsyncRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("api/products")]
        [SwaggerOperation(
            Summary = "Get a List of Products by parameters",
            Description = "Get a List of Products by parameters",
            OperationId = "products.get",
            Tags = new[] { "ProductsEndpoints" })
        ]
        public override async Task<ActionResult<GetProductsResponse>> HandleAsync([FromQuery] GetProductsRequest request, CancellationToken cancellationToken = default)
        {
            var response = new GetProductsResponse(request.CorrelationId());
            if (request.PageIndex.HasValue && request.PageSize.HasValue)
            {
                var productSpec = new ProductSpecification(request.CategoryId, request.StoreId, request.PropertiesId, request.PageIndex.Value, request.PageSize.Value);
                var products = await _productRepository.ListAsync(productSpec);
                response.Products = products.Cast<ProductPreview>().ToList();
                return Ok(response);
            }
            else
            {
                var productSpec = new ProductSpecification(request.CategoryId, request.StoreId, request.PropertiesId);
                var products = await _productRepository.ListAsync(productSpec);
                response.Products = products.Cast<ProductPreview>().ToList();
                return Ok(response);
            }
        }
    }
}
