﻿using ApplicationCore.Entities;
using ApplicationCore.GraphQLEndpoints;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Ardalis.GuardClauses;
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
                Id = userAccess.Id,
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
                Id = userAccess.Id,
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                PhoneNumber = phoneNumber,

            };
            var result = await AddAsync(entity);
                return new RegisterUserPayload(result, token);
        }
        public async Task<bool> AddBasketItem(string userId, string productId)
        {
            try
            {
                using (var context = this._contextFactory.CreateDbContext())
                {
                    var user = await context.Users.Include(x => x.Basket)
                       .ThenInclude(x => x.Items).FirstAsync(x => x.Id == userId);
                    var product = await context.Products.FirstAsync(x => x.Id == productId);
                    if (product != null)
                    {
                        if (user.Basket != null)
                        {
                            user.AddItemToBasket(new BasketItem(1,product));
                        }
                        else
                        {
                           var basket =  await AddBasketAsync(user.Id);
                            if (basket != null)
                            {
                                user = await context.Users.Include(x => x.Basket)
                                    .ThenInclude(x => x.Items).FirstAsync(x => x.Id == userId);
                                user.AddItemToBasket(new BasketItem(1, product));
                            }

                        }
                        await context.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Task<bool> RemoveBasketItem(string userId, string productId)
        {
            throw new NotImplementedException();
        }
        public async Task<Basket> FirstAsync(string userId)
        {
            try
            {
                using (var context = this._contextFactory.CreateDbContext())
                {
                    UserSpecification spec = new UserSpecification(userId);
                    var specificationResult = ApplySpecification(spec, context);
                    var currentUser = await specificationResult.FirstOrDefaultAsync();
                    if (currentUser.Basket != null)
                        return currentUser.Basket;
                    else
                        return await AddBasketAsync(userId);
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Basket> AddBasketAsync(string userId)
        {
            try
            {
                using (var context = this._contextFactory.CreateDbContext())
                {
                    var user = await context.Set<User>().FindAsync(userId);
                    Guard.Against.Null(user, nameof(user));
                    var basket = new Basket() { UserId = user.Id };
                    await context.Baskets.AddAsync(basket);
                    await context.SaveChangesAsync();
                    return basket;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
