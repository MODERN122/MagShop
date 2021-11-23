using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Infrastructure.Constants;
using PublicApi.Providers.AwsS3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PublicApi.GraphQL.Media
{
    [ExtendObjectType(typeof(Mutation))]
    public class ImageMutations
    {
        private IUserRepository _userRepository;
        private IAsyncRepository<Product> _productRepository;


        public ImageMutations(IUserRepository userRepository, IAsyncRepository<Product> productRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
        }
        
        [Authorize(Roles = new string[] { ConstantsAPI.SELLERS, ConstantsAPI.ADMINISTRATORS })]
        public async Task<string> UploadFileAsync(IFile file, string productId, string imageName,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal currentUser)
        {
            using Stream stream = file.OpenReadStream();

            if (stream.Length == 0)
            {
                throw new Exception("Изображение пустое");
            }
            var product = await _productRepository.FirstOrDefaultAsync(new ProductSpecification(productId));
            if (product == null)
            {
                throw new Exception("Товар не найден");
            }
            return await Provider.UploadFromStream(stream, imageName.Split(' ').Last(), $"image/{product.StoreId}/{product.Id}");
        }
    }
}
