using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Infrastructure.Constants;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        [Authorize(Roles = new[] { ConstantsAPI.ADMINISTRATORS, ConstantsAPI.USERS })]
        public async Task<Order> GetOrderById(string id) =>
            await _ordersRepository.FirstAsync(new OrderSpecification(id));

        [Authorize(Roles = new[] { ConstantsAPI.USERS })]
        public async Task<IEnumerable<Order>> GetOrders(
            [Service] UserManager<UserAuthAccess> userManager,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal currentUser, 
            int pageIndex = 0, int pageSize = 20)
        {
            var id = currentUser.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            OrderSpecification orderSpecification = new OrderSpecification(userId: id, pageIndex: pageIndex, pageSize: pageSize);
            var orders = await _ordersRepository.ListAsync(orderSpecification);
            return orders;
        }
    }
}
