﻿using ApplicationCore.Entities;
using ApplicationCore.GraphQLEndpoints;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IUserRepository : IAsyncRepository<User>
    {

        Task<UserPayload> RegisterUserByEmail(string firstName, string lastName, DateTimeOffset birthDate, string email);
        Task<UserPayload> RegisterUserByPhone(string firstName, string lastName, DateTimeOffset birthDate, string phoneNumber, string code);
        Task<UserPayload> RegisterUserByGoogle(string firstName, string lastName, DateTimeOffset birthDate, string accessToken, string email);
        Task<UserPayload> RegisterUserByFacebook(string firstName, string lastName, DateTimeOffset birthDate, string accessToken, string email);

        Task<RegisterSellerPayload> RegisterSellerByPhone(string firstName, string lastName, DateTimeOffset birthDate, string phoneNumber, string code);

        Task<bool> AddBasketItem(string userId, string productId);
        Task<bool> RemoveBasketItem(string userId, string productId);
        Task<Basket> AddBasketAsync(string userId);
        Task<Basket> GetBasketAsync(string userId);
        Task<bool> AddProductToFavoriteAsync(string userId, string productId);
        Task<bool> RemoveProductFromFavoriteAsync(string userId, string productId);
        Task<User> EditUserAsync(EditUserInput userNew);
    }
}
