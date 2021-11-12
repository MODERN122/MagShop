using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.GraphQL.Products
{
    [ExtendObjectType(typeof(Mutation))]
    public class ProductMutaions
    {
        private IAsyncRepository<Product> _productRepository;

        public ProductMutaions(
            IAsyncRepository<Product> productRepository)
        {
            _productRepository = productRepository;
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
            public List<ProductPropertyItemInput> ProductPropertyItems { get; set; }
        }
        public class ProductPropertyItemInput
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


        [GraphQLDescription("Adding Prepublished product")]
        [Authorize(Roles = new string[] { Infrastructure.Constants.ConstantsAPI.SELLERS })]
        public async Task<Product> AddPrePublishProduct(AddProductInput input)
        {
            var product = new Product(input.Name, input.CategoryId, input.Description, input.StoreId);

            var result = await _productRepository.AddAsync(product);
            return result;
        }

        [GraphQLDescription("Adding properties with images to product")]
        [Authorize(Roles = new string[] { Infrastructure.Constants.ConstantsAPI.SELLERS })]
        public async Task<Product> AddProductAsync(
               AddProductInput input)
        {
            try
            {
                ProductSpecification spec = new ProductSpecification(input.Id);
                var product = await _productRepository.FirstOrDefaultAsync(spec);
                if (product == null)
                {
                    product = new Product(input.Id, input.Name, input.CategoryId, input.Description, input.StoreId, input.ProductProperties
                    .Select(x => new ProductProperty()
                    {
                        PropertyId = x.PropertyId,
                        ProductPropertyItems = x.ProductPropertyItems
                            .Select(x => new ProductPropertyItem(x.PropertyItemId, x.Caption, x.PriceNew, x.ImagePath))
                            .ToList() ?? new List<ProductPropertyItem>()
                    })
                    .ToList())
                    {
                        Image = input.Image,
                    };
                    var result = await _productRepository.AddAsync(product);
                    return result;
                }
                else
                {
                    product = new Product(input.Id, input.Name, input.CategoryId, input.Description, input.StoreId, input.ProductProperties
                       .Select(x => new ProductProperty()
                       {
                           PropertyId = x.PropertyId,
                           ProductPropertyItems = x.ProductPropertyItems
                               .Select(x => new ProductPropertyItem(x.PropertyItemId, x.Caption, x.PriceNew, x.ImagePath))
                               .ToList() ?? new List<ProductPropertyItem>()
                       })
                       .ToList())
                    {
                        PublicationDateTime = product.PublicationDateTime,
                        Image = input.Image,
                    };
                    var result = await _productRepository.UpdateAsync(product);
                    if (!result)
                    {
                        throw new Exception("Не смогли обновить товар попробуйте заново");
                    }
                    return product;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
