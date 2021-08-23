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
        private IOrderRepository _ordersRepository;

        public OrderQueries(
            IOrderRepository orderRepository)
        {
            _ordersRepository = orderRepository;
        }
        public async Task<Order> GetOrder(string id) =>
            await _ordersRepository.GetByIdWithItemsAsync(id);

        public async Task<IEnumerable<Order>> GetOrders(int? skip, int? limit)
        {
            OrderSpecification productSpecification = new OrderSpecification(skip??0, limit??int.MaxValue);
            var orders = await _ordersRepository.ListAsync(productSpecification);
            return orders;
        }
    }
}
