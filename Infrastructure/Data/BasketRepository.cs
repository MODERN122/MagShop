using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Ardalis.GuardClauses;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class BasketRepository : EfRepository<Basket>, IBasketRepository
    {
        
        public BasketRepository(MagShopContext dbContext) : base(dbContext)
        {
        }

        public async Task<Basket> FirstAsync(string userId, ISpecification<Basket> spec,  CancellationToken token)
        {
            try
            {
                var specificationResult = ApplySpecification(spec);
                var currentBasket = await specificationResult.FirstAsync();
                if (currentBasket != null)
                {
                    return currentBasket;
                }
                else
                {
                    var user = await _dbContext.Set<User>().FindAsync(userId);
                    Guard.Against.Null(user, nameof(user));
                    var basket = new Basket() { UserId = user.Id };
                    await _dbContext.Set<Basket>().AddAsync(basket, token);
                    await _dbContext.SaveChangesAsync(token);
                    return basket;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Basket> AddBasketAsync(string userId, CancellationToken token)
        {
            try
            {
                var user = await _dbContext.Set<User>().FindAsync(userId);
                Guard.Against.Null(user, nameof(user));
                var basket = new Basket() { UserId = user.Id };
                await _dbContext.Set<Basket>().AddAsync(basket, token);
                await _dbContext.SaveChangesAsync(token);
                return basket;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Basket> TransferBasketAsync(string anonymousUserId, CancellationToken token)
        {
            return new Basket();
        }
    }
}
