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

namespace ApplicationCore.Endpoints.Products
{
    public class GetProduct : BaseAsyncEndpoint<string, GetProductResponse>
    {
        private readonly IAsyncRepository<Product> _productRepository;

        public GetProduct(IAsyncRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("api/products/{id}")]
        [SwaggerOperation(
            Summary = "Get a Product by id",
            Description = "Get a Product by id",
            OperationId = "product.get",
            Tags = new[] { "ProductsEndpoints" })
        ]
        public override async Task<ActionResult<GetProductResponse>> HandleAsync(string id, CancellationToken cancellationToken = default)
        {
            var productSpec = new ProductSpecification(id);
            var product = await _productRepository.FirstAsync(productSpec);
            return Ok(new GetProductResponse { Product = product }); ;
        }
    }
}
