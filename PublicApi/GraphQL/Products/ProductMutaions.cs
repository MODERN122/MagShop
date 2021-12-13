using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Ardalis.GuardClauses;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Infrastructure.Constants;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PublicApi.GraphQL.Products
{
    [ExtendObjectType(typeof(Mutation))]
    public class ProductMutaions
    {
        private IAsyncRepository<Product> _productRepository;
        private IAsyncRepository<ProductProperty> _productPropertyRepository;

        public ProductMutaions(
            IAsyncRepository<Product> productRepository,
            IAsyncRepository<ProductProperty> productPropertyRepository)
        {
            _productRepository = productRepository;
            _productPropertyRepository = productPropertyRepository;
        }

        [GraphQLDescription("Product input for adding Prepublished product")]
        public record AddProductInput(
            [GraphQLNonNullType]
            string Name,
            [GraphQLNonNullType]
            string StoreId,
            [GraphQLNonNullType]
            string CategoryId,
            [GraphQLNonNullType]
            string Description,
            [GraphQLNonNullType]
            string Image,
            List<string> ImagePaths,
            string Id,
            List<ProductPropertiesInput> ProductProperties);

        public class AddProductPayload
        {
            public AddProductPayload(Product product)
            {
                Product = product;
            }

            public Product Product { get; }
        }

        public class ProductPropertiesInput
        {
            [GraphQLNonNullType]
            public string PropertyId { get; set; }
            [GraphQLNonNullType]
            public List<ProductPropertyIItemInput> ProductPropertyItemsInput { get; set; }
        }

        public class ProductPropertyIItemInput
        {
            [GraphQLNonNullType]
            public string Caption { get; set; }
            [GraphQLNonNullType]
            public string PropertyItemId { get; set; }
            [GraphQLNonNullType]
            public int PriceNew { get; set; }
            [GraphQLNonNullType]
            public string ImagePath { get; set; }
            public int PriceOld { get; set; }
        }

        [GraphQLDescription("Add Prepublished product")]
        [Authorize(Roles = new string[] { ConstantsAPI.SELLERS })]
        public async Task<Product> AddPrePublishProduct(AddProductInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal currentUser)
        {
            if (input == null)
            {
                throw new Exception("input was null");
            }
            var product = new Product(input.Name, input.CategoryId, input.Description, input.StoreId, currentUser.Claims.First().Value);
            product.Images = input.ImagePaths.Select(x => new Image(product.Id, x)).ToList();

            var result = await _productRepository.AddAsync(product);
            return result;
        }

        [GraphQLDescription("Add properties with images to product")]
        [Authorize(Roles = new string[] { ConstantsAPI.SELLERS })]
        public async Task<Product> AddProductPropertiesToProductAsync(string productId,
               List<ProductPropertiesInput> input)
        {
            try
            {
                var spec = new ProductSpecification(productId);
                var product = await _productRepository.FirstOrDefaultAsync(spec);
                Guard.Against.Null(product, nameof(product));
                product.SetProductProperties(input.Select(x => new ProductProperty()
                {
                    ProductId = productId,
                    PropertyId = x.PropertyId,
                    ProductPropertyItems = x.ProductPropertyItemsInput
                        .Select(x => new ProductPropertyItem(x.PropertyItemId, x.Caption, x.PriceNew, x.ImagePath, product.Store.SellerId)
                        {
                            PriceOld = x.PriceOld,
                        })
                        .ToList() ?? new List<ProductPropertyItem>()
                })
                .ToList());
                product.SetProductIsActive(true);
                var result = await _productRepository.UpdateEntryAsync(product);
                Guard.Against.Default(result, nameof(result));
                return await _productRepository.FirstOrDefaultAsync(new ProductSpecification(product.Id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        [GraphQLDescription("Delete product")]
        [Authorize(Roles = new string[] { ConstantsAPI.SELLERS, ConstantsAPI.ADMINISTRATORS })]
        public async Task<bool> DeleteProductAsync(string productId)
        {
            try
            {
                var spec = new ProductSpecification(productId);
                var product = await _productRepository.FirstOrDefaultAsync(spec);
                Guard.Against.Null(product, nameof(product));
                product.SetProductIsActive(false);
                var result = await _productRepository.UpdateEntryAsync(product);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }


    }
}
