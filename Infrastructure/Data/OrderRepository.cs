using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class OrderRepository : EfRepository<Order>, IOrderRepository
    {
        public OrderRepository(IDbContextFactory<MagShopContext> dbContext) : base(dbContext)
        {
        }

        public async Task<Order> GetByIdWithItemsAsync(string id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var res = await context.Set<Order>()
                    .Include(x=>x.ShipToAddress)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(x => x.Id == id);
                return res;
            }
        }
    }
}
