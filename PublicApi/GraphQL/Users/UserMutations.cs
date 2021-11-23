﻿using ApplicationCore.Entities;
using ApplicationCore.GraphQLEndpoints;
using ApplicationCore.Interfaces;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Infrastructure.Constants;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PublicApi.GraphQL.Users
{
    [ExtendObjectType(typeof(Mutation))]
    public class UserMutations
    {
        private IUserRepository _userRepository;

        public UserMutations(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public record RegisterUserByPhoneInput(
            string FirstName,
            string LastName,
            DateTimeOffset BirthDate,
            string PhoneNumber,
            string Code);
        public record RegisterUserByGoogleInput(
            string FirstName,
            string LastName,
            DateTimeOffset BirhDate,
            string AccessToken,
            string Email);

        public async Task<RegisterUserPayload> RegisterUserByPhoneAsync(
               RegisterUserByPhoneInput input)
        {
                var result = await _userRepository.RegisterUserByPhone(input.FirstName, input.LastName, input.BirthDate, input.PhoneNumber, input.Code);
                return result;
        }

        public async Task<RegisterUserPayload> RegisterUserByGoogleAsync(
               RegisterUserByGoogleInput input)
        {
                var result = await _userRepository.RegisterUserByGoogle(input.FirstName, input.LastName, input.BirhDate, input.AccessToken, input.Email);
                return result;
        }

        public async Task<RegisterSellerPayload> RegisterSellerByPhoneAsync(
              RegisterUserByPhoneInput input)
        {
                var result = await _userRepository.RegisterSellerByPhone(input.FirstName, input.LastName, input.BirthDate, input.PhoneNumber, input.Code);
                return result;
        }
        [Authorize(Roles = new string[] { ConstantsAPI.USERS})]
        public async Task<bool> AddProductToFavoriteAsync(string productId,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal currentUser)
        {
            var result = await _userRepository.AddProductToFavoriteAsync(currentUser.Claims.First().Value, productId);
            return result;
        }
        [Authorize(Roles = new string[] { ConstantsAPI.USERS })]
        public async Task<bool> RemoveProductFromFavoriteAsync(string productId,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal currentUser)
        {
            var result = await _userRepository.RemoveProductFromFavoriteAsync(currentUser.Claims.First().Value, productId);
            return result;
        }

        [Authorize(Roles = new string[] { ConstantsAPI.USERS, ConstantsAPI.SELLERS })]
        public async Task<User> EditUserAsync(EditUserInput userNew, [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal currentUser)
        {
            if (currentUser.Claims.First().Value != userNew.Id)
            {
                throw new Exception("Вам низзя изменять этого пользователя");
            }
            var result = await _userRepository.EditUserAsync(userNew);
            return result;
        }
    }
}
