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

        public BasketRepository(IDbContextFactory<MagShopContext> dbContext) : base(dbContext)
        {
        }

        public async Task<Basket> FirstAsync(string userId, ISpecification<Basket> spec, CancellationToken token)
        {
            try
            {
                using (var context = this._contextFactory.CreateDbContext())
                {
                    var specificationResult = ApplySpecification(spec, context);
                    var currentBasket = await specificationResult.FirstAsync();
                    if (currentBasket != null)
                    {
                        return currentBasket;
                    }
                    else
                    {
                        var user = await context.Set<User>().FindAsync(userId);
                        Guard.Against.Null(user, nameof(user));
                        var basket = new Basket() { UserId = user.Id };
                        await context.Set<Basket>().AddAsync(basket, token);
                        await context.SaveChangesAsync(token);
                        return basket;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Basket> AddBasketAsync(string userId, CancellationToken token = default)
        {
            try
            {
                using (var context = this._contextFactory.CreateDbContext())
                {
                    var user = await context.Set<User>().FindAsync(userId);
                    Guard.Against.Null(user, nameof(user));
                    var basket = new Basket() { UserId = user.Id };
                    await context.Set<Basket>().AddAsync(basket, token);
                    await context.SaveChangesAsync(token);
                    return basket;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Basket> TransferBasketAsync(string anonymousUserId, CancellationToken token = default)
        {
            return new Basket();
        }
    }
}
