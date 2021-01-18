using ApplicationCore.Entities;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CloudMarket.Interfaces
{
    public interface IMagShopAPI
    {
        [Get("api/products")]
        Task<List<Product>> GetProducts();
        [Post("api/authentication")]
        Task<AuthenticationResponse>
        [Post("api/products")]
        [Put("api/products/{id}")]
        [Get("api/products")]
        [Get("api/products/{id}")]
        [Post("api/users/")]
        [Delete("api/products/{id}")]
    } 
}
