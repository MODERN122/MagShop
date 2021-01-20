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
        [Get("/api/products")]
        Task<GetProductsResponse> GetProductsAsync([Query(CollectionFormat.Multi)] GetProductsRequest request, CancellationToken ctx);
        [Get("/api/products/{id}")]
        Task<GetProductResponse> GetProductAsync(string id, CancellationToken ctx);
        [Post("/api/authentication")]
        Task<AuthenticationResponse> AuthenticateAsync([Body] AuthenticationRequest request, CancellationToken ctx);
        [Post("/api/users/")]
        Task<CreateUserResponse> CreateUserAsync([Body] CreateUserRequest request, CancellationToken ctx);
        [Post("/api/products")]
        [Headers("Authorization: Bearer")]
        Task<CreateProductResponse> CreateProductAsync([Body] CreateProductRequest request, CancellationToken ctx);
        [Put("/api/products/{id}")]
        [Headers("Authorization: Bearer")]
        Task<PutProductResponse> UpdateProductAsync(string id, [Body] PutProductRequest request, CancellationToken ctx);
        [Get("/api/users/{id}/basket")]
        [Headers("Authorization: Bearer")]
        Task<GetBasketResponse> GetUserBasketAsync(string id, CancellationToken ctx);
        [Delete("/api/products/{id}")]
        [Headers("Authorization: Bearer")]
        Task<DeleteUserResponse> DeleteUserAsync(string id, CancellationToken ctx);
    }
}
