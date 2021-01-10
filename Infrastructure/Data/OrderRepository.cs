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
        public OrderRepository(MagShopContext dbContext) : base(dbContext)
        {
        }

        public Task<Order> GetByIdWithItemsAsync(string id)
        {
            return _dbContext.Orders
                .Include(o => o.Items)
                .ThenInclude(i=>i.ProducOrdered)
                .FirstOrDefaultAsync(x => x.OrderId == id);
        }
    }
}
