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
        [Get("/api/products?CategoryId={categoryId}&StoreId={storeId}&PageSize={pageSize}&" +
            "PageIndex={pageIndex}")]
        Task<List<ProductPreview>> GetProductsAsync(string categoryId, string storeId, int? pageSize,
            int? pageIndex, [Query(CollectionFormat.Multi)] List<string> propertiesId, CancellationToken ctx);
        [Get("/api/products/{id}")]
        Task<Product> GetProductAsync(string id, CancellationToken ctx);
        [Post("/api/authentication")]
        Task<AuthenticationResponse> AuthenticateAsync([Body] AuthenticationRequest request, CancellationToken ctx);
        [Post("/api/users/")]
        Task<User> CreateUserAsync([Body] CreateUserRequest request, CancellationToken ctx);
        [Post("/api/products")]
        [Headers("Authorization: Bearer")]
        Task<Product> CreateProductAsync([Body] CreateProductRequest request, CancellationToken ctx);
        [Put("/api/products/{id}")]
        [Headers("Authorization: Bearer")]
        Task<Product> UpdateProductAsync(string id, [Body] PutProductRequest request, CancellationToken ctx);
        [Get("/api/users/{id}/basket")]
        [Headers("Authorization: Bearer")]
        Task<Basket> GetUserBasketAsync(string id, CancellationToken ctx);
        [Delete("/api/products/{id}")]
        [Headers("Authorization: Bearer")]
        Task<string> DeleteUserAsync(string id, CancellationToken ctx);
    }
}
