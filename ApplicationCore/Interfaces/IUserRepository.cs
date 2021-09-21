using ApplicationCore.Entities;
using ApplicationCore.GraphQLEndpoints;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IUserRepository : IAsyncRepository<User>
    {

        Task<RegisterUserPayload> RegisterUserByEmail(string firstName, string lastName, DateTimeOffset birthDate, string email);
        Task<RegisterUserPayload> RegisterUserByPhone(string firstName, string lastName, DateTimeOffset birthDate, string phoneNumber, string code);
        Task<RegisterUserPayload> RegisterUserByGoogle(string firstName, string lastName, DateTimeOffset birthDate, string accessToken, string email);
        Task<RegisterUserPayload> RegisterUserByFacebook(string firstName, string lastName, DateTimeOffset birthDate, string accessToken, string email);

        Task<RegisterSellerPayload> RegisterSellerByPhone(string firstName, string lastName, DateTimeOffset birthDate, string phoneNumber, string code);

        Task<bool> AddBasketItem(string userId, string productId);
        Task<bool> RemoveBasketItem(string userId, string productId);
        Task<Basket> AddBasketAsync(string userId);
        Task<Basket> FirstAsync(string userId);
        Task<bool> AddProductToFavoriteAsync(string userId, string productId);
        Task<bool> RemoveProductFromFavoriteAsync(string userId, string productId);
    }
}
