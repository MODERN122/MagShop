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
    }
}
