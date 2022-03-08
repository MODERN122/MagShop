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

        Task<UserPayload> RegisterUserByEmail(string firstName, string lastName, DateTime birthDate, string email);
        Task<UserPayload> RegisterUserByPhone(string firstName, string lastName, DateTime birthDate, string phoneNumber, string code);
        Task<UserPayload> RegisterUserByGoogle(string firstName, string lastName, DateTime birthDate, string accessToken, string email);
        Task<UserPayload> RegisterUserByFacebook(string firstName, string lastName, DateTime birthDate, string accessToken, string email);

        Task<RegisterSellerPayload> RegisterSellerByPhone(string firstName, string lastName, DateTime birthDate, string phoneNumber, string code);

        Task<Basket> AddBasketItem(string userId, string productId);
        Task<Basket> AddBasketItem(string userId, string productId, List<string> selectedProductPropertyItemIds);
        Task<Basket> RemoveBasketItem(string userId, string productId);
        Task<Basket> SubstractBasketItem(string userId, string baasketItemId);
        Task<Basket> AddBasketAsync(string userId);
        Task<Basket> GetBasketAsync(string userId);
        Task<bool> AddProductToFavoriteAsync(string userId, string productId);
        Task<bool> RemoveProductFromFavoriteAsync(string userId, string productId);
        Task<User> EditUserAsync(EditUserInput userNew);
    }
}
