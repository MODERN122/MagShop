using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderRepository : EfRepository<Order>, IOrderRepository
    {
        public OrderRepository(IDbContextFactory<MagShopContext> dbContext) : base(dbContext)
        {
        }

        public async Task<Order> CreateOrder(List<string> basketItemIds, string transactionId, string addressId, string userId)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var basketItems = await context.BasketItems
                    .Include(x=>x.Product)
                    .Where(x=>basketItemIds.Contains(x.Id))
                    .ToListAsync();
                var userAddress = await context.Addresses
                    .FirstOrDefaultAsync(x=>x.Id == addressId);
                userAddress.Id = Guid.NewGuid().ToString();
                var addedOrderAddress = await context.AddAsync(userAddress);
                await context.SaveChangesAsync();
                var orderAddress = await context.Addresses.FirstOrDefaultAsync(y=>y.Id == userAddress.Id);
                List<OrderItem> items = new List<OrderItem>();
                foreach (var item in basketItems)
                {
                    var orderItem = await ConvertBasketItemToOrderItem(item);
                    items.Add(orderItem);                
                }
                
                var order = new Order(orderAddress.Id, items, userId, transactionId);
                
                await context.AddAsync(order);
                await context.SaveChangesAsync();
                var addedOrder = await FirstAsync(new OrderSpecification(id:order.Id));
                return addedOrder;
            }
        }

        private async Task<OrderItem> ConvertBasketItemToOrderItem(BasketItem basketItem)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var productPropertyItems = await context.ProductPropertyItems
                    .Where(x => basketItem.SelectedProductPropertyItemIds.Contains(x.Id))
                    .ToListAsync();
                var orderItem = new OrderItem(basketItem.Quantity, basketItem.Product, productPropertyItems);
                return orderItem;
            }
        }
    }
}
