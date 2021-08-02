using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using HotChocolate;
using HotChocolate.Types;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.GraphQL.Orders
{
    [ExtendObjectType(typeof(Query))]
    public class OrderQueries
    {
        public async Task<Order> GetOrder(string id,
            [Service] IOrderRepository ordersRepository) =>
            await ordersRepository.GetByIdWithItemsAsync(id);

        public async Task<IEnumerable<Order>> GetOrders(int? limit,
            [Service]IOrderRepository ordersRepository)
        {
            OrderSpecification productSpecification = new OrderSpecification(limit??int.MaxValue);
            var orders = await ordersRepository.ListAsync(productSpecification);
            return orders;
        }
    }
}
