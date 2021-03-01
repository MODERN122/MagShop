using ApplicationCore.Endpoints;
using ApplicationCore.Endpoints.Authentication;
using ApplicationCore.Endpoints.Baskets;
using ApplicationCore.Endpoints.Products;
using ApplicationCore.Endpoints.Users;
using ApplicationCore.Entities;
using CloudMarket.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudMarket.Interfaces
{
    public interface IMagShopApi
    {
        [Get(EndpointUrlConstants.PRODUCTS_URL)]
        Task<GetProductsResponse> GetProductsAsync([Query(CollectionFormat.Multi)] GetProductsRequest request, CancellationToken ctx);
        [Get(EndpointUrlConstants.PRODUCT_URL)]
        Task<GetProductResponse> GetProductAsync(string id, CancellationToken ctx);
        [Post(EndpointUrlConstants.AUTHENTICATION_URL)]
        Task<AuthenticationResponse> AuthenticateAsync([Body] AuthenticationRequest request, CancellationToken ctx);
        [Post(EndpointUrlConstants.USERS_URL)]
        Task<CreateUserResponse> CreateUserAsync([Body] CreateUserRequest request, CancellationToken ctx);
        [Post(EndpointUrlConstants.PRODUCTS_URL)]
        [Headers("Authorization: Bearer")]
        Task<CreateProductResponse> CreateProductAsync([Body] CreateProductRequest request, CancellationToken ctx);
        [Put(EndpointUrlConstants.PRODUCT_URL)]
        [Headers("Authorization: Bearer")]
        Task<PutProductResponse> UpdateProductAsync(string id, [Body] PutProductRequest request, CancellationToken ctx);
        [Get(EndpointUrlConstants.BASKET_URL)]
        [Headers("Authorization: Bearer")]
        Task<GetBasketResponse> GetUserBasketAsync(string id, CancellationToken ctx);
        [Delete(EndpointUrlConstants.PRODUCT_URL)]
        [Headers("Authorization: Bearer")]
        Task<DeleteUserResponse> DeleteProductAsync(string id, CancellationToken ctx);
    }
}
