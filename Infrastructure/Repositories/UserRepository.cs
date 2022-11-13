using ApplicationCore.Entities;
using ApplicationCore.GraphQLEndpoints;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Ardalis.GuardClauses;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        private readonly IUserAuthService _tokenClaimsService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        public UserRepository(IDbContextFactory<MagShopContext> dbContext, IServiceProvider serviceProvider,
            IUserAuthService tokenClaimsService, IMapper mapper) : base(dbContext)
        {
            _tokenClaimsService = tokenClaimsService;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }
        public async Task<RegisterSellerPayload> RegisterSellerByPhone(string firstName, string lastName, DateTime birthDate, string phoneNumber, string code)
        {
            var userAccess = new UserAuthAccess(phoneNumber);
            using var context = this._contextFactory.CreateDbContext();
            var token = await _tokenClaimsService.RegisterSellerAsync(userAccess);

            var entity = new Seller(phoneNumber)
            {
                Id = userAccess.Id,
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate.ToUniversalTime(),
                PhoneNumber = phoneNumber,
            };
            var result = await AddAsync(entity);
            return new RegisterSellerPayload(result as Seller, token);
        }

        public async Task<UserPayload> RegisterUserByEmail(string firstName, string lastName, DateTime birthDate, string email)
        {
            throw new NotImplementedException();
        }

        public async Task<UserPayload> RegisterUserByFacebook(string firstName, string lastName, DateTime birthDate, string accessToken, string email)
        {
            throw new NotImplementedException();
        }

        public async Task<UserPayload> RegisterUserByGoogle(string firstName, string lastName, DateTime birthDate, string accessToken, string email)
        {
            var userAccess = new UserAuthAccess(email);
            userAccess.GoogleToken = accessToken;
            using var context = this._contextFactory.CreateDbContext();
            var token = await _tokenClaimsService.RegisterUserAsync(userAccess);

            var entity = new User(email)
            {
                Id = userAccess.Id,
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate.ToUniversalTime(),
                Email = email,
            };

            var result = await AddAsync(entity);
            return new UserPayload(result, token);
        }

        public async Task<UserPayload> RegisterUserByPhone(string firstName, string lastName, DateTime birthDate, string phoneNumber, string code)
        {
            var userAccess = new UserAuthAccess(phoneNumber);
            using var context = this._contextFactory.CreateDbContext();
            var token = await _tokenClaimsService.RegisterUserAsync(userAccess);

            var entity = new User(phoneNumber)
            {
                Id = userAccess.Id,
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate.ToUniversalTime(),
                PhoneNumber = phoneNumber,

            };
            var result = await AddAsync(entity);
            return new UserPayload(result, token);
        }
        public async Task<Basket> AddBasketItem(string userId, string productId)
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
                        user.AddItemToBasket(new BasketItem(1, product));
                    }
                    else
                    {
                        var basket = await AddBasketAsync(user.Id);
                        if (basket != null)
                        {
                            user = await context.Users.Include(x => x.Basket)
                                .ThenInclude(x => x.Items).FirstAsync(x => x.Id == userId);
                            user.AddItemToBasket(new BasketItem(1, product));
                        }

                    }
                    await context.SaveChangesAsync();
                    return await GetBasketAsync(userId);
                }
                throw new Exception("ProductNotFound!");
            }
        }

        public async Task<Basket> AddBasketItem(string userId, string productId, List<string> selectedProductPropertyItemIds)
        {
            using (var context = this._contextFactory.CreateDbContext())
            {
                var user = await context.Users.Include(x => x.Basket)
                   .ThenInclude(x => x.Items)
                   .FirstAsync(x => x.Id == userId);
                var product = await context.Products
                    .Include(x => x.ProductProperties)
                    .ThenInclude(x => x.ProductPropertyItems)
                    .FirstAsync(x => x.Id == productId);
                if (product != null)
                {
                    var productPropertyItemGroups = product.ProductProperties.Select(x => x.ProductPropertyItems.Select(y => y.Id));
                    var productPropertyItemIds = new List<string>();
                    foreach (var productPropertyItemGroup in productPropertyItemGroups)
                    {
                        productPropertyItemIds.AddRange(productPropertyItemGroup);
                    }
                    var enteredProductPropertyItemIds = selectedProductPropertyItemIds.Where(x => productPropertyItemIds.Contains(x)).ToList();

                    if (enteredProductPropertyItemIds.Count == 0)
                    {
                        throw new Exception("Selected product property item ids were not found in product");
                    }

                    if (user.Basket != null)
                    {
                        user.AddItemToBasket(new BasketItem(1, product, enteredProductPropertyItemIds));
                    }
                    else
                    {
                        var basket = await AddBasketAsync(user.Id);
                        if (basket != null)
                        {
                            user = await context.Users.Include(x => x.Basket)
                                .ThenInclude(x => x.Items).FirstAsync(x => x.Id == userId);
                            user.AddItemToBasket(new BasketItem(1, product, enteredProductPropertyItemIds));
                        }

                    }
                    await context.SaveChangesAsync();
                    return await GetBasketAsync(userId);
                }
                throw new Exception("ProductNotFound!");
            }
        }
        public async Task<Basket> SubstractBasketItem(string userId, string basketItemId)
        {
            using (var context = this._contextFactory.CreateDbContext())
            {
                var user = await context.Users.Include(x => x.Basket)
                   .ThenInclude(x => x.Items)
                   .FirstAsync(x => x.Id == userId);
                var basketItem = await context.BasketItems.Include(x => x.Product).FirstAsync(x => x.Id == basketItemId);
                if (basketItem != null)
                {
                    if (user.Basket != null)
                    {
                        user.SubstractItemFromBasket(basketItem);
                    }
                    else
                    {
                        var basket = await AddBasketAsync(user.Id);
                        if (basket != null)
                        {
                            user = await context.Users.Include(x => x.Basket)
                                .ThenInclude(x => x.Items).FirstAsync(x => x.Id == userId);
                        }

                    }
                    await context.SaveChangesAsync();
                    return await GetBasketAsync(userId);
                }
                throw new Exception("ProductNotFound!");
            }
        }

        public async Task<Basket> RemoveBasketItem(string userId, string basketItemId)
        {
            using (var context = this._contextFactory.CreateDbContext())
            {
                var user = await context.Users.Include(x => x.Basket)
                   .ThenInclude(x => x.Items).FirstAsync(x => x.Id == userId);
                var basketItem = await context.BasketItems.FirstAsync(x => x.Id == basketItemId);
                if (basketItem != null)
                {
                    if (user.Basket != null)
                    {
                        user.RemoveItemFromBasket(basketItem);
                    }
                    else
                    {
                        var basket = await AddBasketAsync(user.Id);
                        if (basket != null)
                        {
                            user = await context.Users.Include(x => x.Basket)
                                .ThenInclude(x => x.Items).FirstAsync(x => x.Id == userId);
                        }

                    }
                    await context.SaveChangesAsync();
                    return await GetBasketAsync(userId);
                }
                throw new Exception("ProductNotFound!");
            }
        }

        public async Task<Basket> GetBasketAsync(string userId)
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

        public async Task<Basket> AddBasketAsync(string userId)
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

        public async Task<bool> AddProductToFavoriteAsync(string userId, string productId)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var user = await context.Set<User>().FirstOrDefaultAsync(x => x.Id == userId);
                Guard.Against.Null(user, nameof(user));
                var product = await context.Products.FindAsync(productId);
                if (product != null)
                {
                    var result = user.AddProductToFavorite(productId);
                    if (!result)
                        return false;
                    context.Users.Update(user);
                    await context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new Exception("Товар не найден");
                }
            }
        }
        public async Task<bool> RemoveProductFromFavoriteAsync(string userId, string productId)
        {
            using (var context = this._contextFactory.CreateDbContext())
            {
                var user = await context.Set<User>().FirstOrDefaultAsync(x => x.Id == userId);
                Guard.Against.Null(user, nameof(user));
                var product = await context.Products.FindAsync(productId);
                if (product != null)
                {
                    var result = user.RemoveProductFromFavorite(productId);
                    if (!result)
                        return false;
                    context.Users.Update(user);
                    await context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new Exception("Товар не найден");
                }
            }
        }

        public async Task<User> EditUserAsync(EditUserInput userNew)
        {
            using (var context = this._contextFactory.CreateDbContext())
            {
                var user = await context.Set<User>().FirstOrDefaultAsync(x => x.Id == userNew.Id);
                if (user != null)
                {
                    _mapper.Map(userNew, user);
                    context.Users.Update(user);
                    await context.SaveChangesAsync();
                    user = await context.Set<User>().FirstOrDefaultAsync(x => x.Id == userNew.Id);
                    return user;
                }
                return default;
            }
        }

        public async Task<Address> UpdateAddressAsync(Address address, string userId)
        {
            using (var context = this._contextFactory.CreateDbContext())
            {
                var addressOld = address.Id == "" ? null : await context.Set<Address>().FirstOrDefaultAsync(x => x.Id == address.Id);
                if (addressOld != null)
                {
                    _mapper.Map(address, addressOld);
                    context.Addresses.Update(addressOld);
                    var userAddressOld = await context.Set<UserAddress>().FirstOrDefaultAsync(x => x.AddressId == address.Id && x.UserId == userId);
                    if (userAddressOld == null)
                    {
                        await context.UserAddresses.AddAsync(new UserAddress(userId, addressOld.Id));
                    }
                    await context.SaveChangesAsync();
                    addressOld = await context.Set<Address>().FirstOrDefaultAsync(x => x.Id == address.Id);
                    return addressOld;
                }
                else
                {
                    address.Id = Guid.NewGuid().ToString();
                    await context.Addresses.AddAsync(address);
                    await context.UserAddresses.AddAsync(new UserAddress(userId, address.Id));
                    await context.SaveChangesAsync();
                    addressOld = await context.Set<Address>().FirstOrDefaultAsync(x => x.Id == address.Id);
                    return addressOld;
                }
            }
        }
    }
}
