using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IOrderRepository : IAsyncRepository<Order>
    {
        Task<Order> CreateOrder(List<string> basketItemIds, string transactionId, string addressId, string userId);
    }
}
