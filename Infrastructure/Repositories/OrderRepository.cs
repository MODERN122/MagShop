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

        public async Task<Order> CreateOrder(List<string> basketItemIds, string transactionId, string creditCardId, string addressId, string deliveryCourierId, double paymentAmount, string userId)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var basketItems = await context.BasketItems
                    .Include(x => x.Product)
                    .Where(x => basketItemIds.Contains(x.Id))
                    .ToListAsync();

                if (basketItems.Count == 0)
                {
                    throw new Exception("Basket items not found!");
                }

                var userAddress = await context.Addresses
                    .FirstOrDefaultAsync(x => x.Id == addressId);

                if (userAddress == null)
                {
                    throw new Exception("User address not found!");
                }

                userAddress.Id = Guid.NewGuid().ToString();
                var addedOrderAddress = await context.AddAsync(userAddress);

                var creditCard = await context.CreditCards
                    .FirstOrDefaultAsync(x => x.Id == creditCardId);

                if (creditCard == null)
                {
                    throw new Exception("Credit card not found!");
                }

                creditCard.Id = Guid.NewGuid().ToString();
                var addedOrderCredidCard = await context.AddAsync(creditCard);

                var transaction = new Transaction(transactionId, PaymentType.Unknown, paymentAmount);
                var addedTransaction = await context.AddAsync(transaction);

                await context.SaveChangesAsync();
                //TODO Can optimize
                var orderAddress = await context.Addresses.FirstOrDefaultAsync(y => y.Id == userAddress.Id);
                var orderCreditCard = await context.CreditCards.FirstOrDefaultAsync(y => y.Id == creditCard.Id);
                var orderTransaction = await context.Transactions.FirstOrDefaultAsync(y => y.Id == transaction.Id);

                List<OrderItem> items = new List<OrderItem>();
                foreach (var item in basketItems)
                {
                    var orderItem = await ConvertBasketItemToOrderItem(item);
                    items.Add(orderItem);
                }

                var order = new Order(orderAddress.Id, items, userId, orderTransaction.Id, orderCreditCard.Id, deliveryCourierId);

                try
                {
                    await context.AddAsync(order);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw;
                }
                var addedOrder = await FirstOrDefaultAsync(new OrderSpecification(id: order.Id));

                if (addedOrder != null)
                {
                    context.BasketItems.RemoveRange(basketItems);
                }

                await context.SaveChangesAsync();

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
