using ApplicationCore.Entities;
using ApplicationCore.GraphQLEndpoints;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        private readonly IUserAuthService _tokenClaimsService;
        private readonly IServiceProvider _serviceProvider;
        public UserRepository(IDbContextFactory<MagShopContext> dbContext, IServiceProvider serviceProvider,
            IUserAuthService tokenClaimsService) : base(dbContext)
        {
            _tokenClaimsService = tokenClaimsService;
            _serviceProvider = serviceProvider;
        }
        public async Task<RegisterSellerPayload> RegisterSellerByPhone(string firstName, string lastName, DateTimeOffset birthDate, string phoneNumber, string code)
        {
            var userAccess = new UserAuthAccess(phoneNumber);
            using var context = this._contextFactory.CreateDbContext();
            var token = await _tokenClaimsService.RegisterSellerAsync(userAccess);

            var entity = new Seller()
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                PhoneNumber = phoneNumber,

            };
            var result = await AddAsync(entity);
            return new RegisterSellerPayload(result as Seller, token);
        }

        public async Task<RegisterUserPayload> RegisterUserByEmail(string firstName, string lastName, DateTimeOffset birthDate, string email)
        {
            throw new NotImplementedException();
        }

        public async Task<RegisterUserPayload> RegisterUserByFacebook(string firstName, string lastName, DateTimeOffset birthDate, string accessToken, string email)
        {
            throw new NotImplementedException();
        }

        public async Task<RegisterUserPayload> RegisterUserByGoogle(string firstName, string lastName, DateTimeOffset birthDate, string accessToken, string email)
        {
            var userAccess = new UserAuthAccess(email);
            userAccess.GoogleToken = accessToken;
            using var context = this._contextFactory.CreateDbContext();
            var token = await _tokenClaimsService.RegisterUserAsync(userAccess);

            var entity = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                Email = email,

            };

            var result = await AddAsync(entity);
            return new RegisterUserPayload(result, token);
        }

        public async Task<RegisterUserPayload> RegisterUserByPhone(string firstName, string lastName, DateTimeOffset birthDate, string phoneNumber, string code)
        {
            var userAccess = new UserAuthAccess(phoneNumber);
            using var context = this._contextFactory.CreateDbContext();
            var token = await _tokenClaimsService.RegisterUserAsync(userAccess);

            var entity = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                PhoneNumber = phoneNumber,

            };
            var result = await AddAsync(entity);
                return new RegisterUserPayload(result, token);
        }
    }
}
