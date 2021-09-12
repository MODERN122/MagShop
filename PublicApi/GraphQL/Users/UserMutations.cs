using ApplicationCore.Entities;
using ApplicationCore.GraphQLEndpoints;
using ApplicationCore.Interfaces;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.GraphQL.Users
{
    [ExtendObjectType(typeof(Mutation))]
    public class UserMutaions
    {
        private IUserRepository _userRepository;

        public UserMutaions(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public record RegisterUserByPhoneInput(
            string FirstName,
            string LastName,
            DateTimeOffset BirhDate,
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
            try
            {
                var result = await _userRepository.RegisterUserByPhone(input.FirstName, input.LastName, input.BirhDate, input.PhoneNumber, input.Code);
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<RegisterUserPayload> RegisterUserByGoogleAsync(
               RegisterUserByGoogleInput input)
        {
            try
            {
                var result = await _userRepository.RegisterUserByGoogle(input.FirstName, input.LastName, input.BirhDate, input.AccessToken, input.Email);
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<RegisterSellerPayload> RegisterSellerByPhoneAsync(
              RegisterUserByPhoneInput input)
        {
            try
            {
                var result = await _userRepository.RegisterSellerByPhone(input.FirstName, input.LastName, input.BirhDate, input.PhoneNumber, input.Code);
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
