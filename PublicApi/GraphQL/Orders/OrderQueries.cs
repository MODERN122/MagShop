using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
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

        [Authorize(Roles = new[] { "Administrators" })]
        public async Task<IEnumerable<Order>> GetOrders(int pageIndex = 0, int pageSize = 20)
        {
            OrderSpecification productSpecification = new OrderSpecification(pageIndex, pageSize);
            var orders = await _ordersRepository.ListAsync(productSpecification);
            return orders;
        }
    }
}
