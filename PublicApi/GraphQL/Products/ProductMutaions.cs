using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
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
    //[ExtendObjectType(
    //nameof(ProductProperty),
    //IgnoreProperties = new[] { nameof(ProductProperty.ProductId), nameof(ProductProperty.Product) })]
    public class ProductMutaions
    {
        private IAsyncRepository<Product> _productRepository;

        public ProductMutaions(
            IAsyncRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

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
    [GraphQLNonNullType]
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
            public string ImagePath { get; set; }
        }
        
        [Authorize(Roles =new string[] { Infrastructure.Constants.ConstantsAPI.SELLERS})]
        public async Task<Product> AddProductAsync(
               AddProductInput input)
        {
            var product = new Product(input.Name, input.CategoryId, input.Description, input.StoreId, input.ProductProperties
                    .Select(x => new ProductProperty()
                    {
                        PropertyId = x.PropertyId,
                        ProductPropertyItems = x.ProductPropertyItems?
                            .Select(x => new ProductPropertyItem(x.PropertyItemId, x.Caption))
                            .ToList()??new List<ProductPropertyItem>()
                    })
                    .ToList())
            { 
                Image = input.Image,
                //TODO create url for data
                Url = ""
            };
            try
            {
                var result = await _productRepository.AddAsync(product);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
