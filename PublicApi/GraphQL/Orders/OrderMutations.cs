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
    [ExtendObjectType(typeof(Mutation))]
    public class OrderMutations
    {
        private IOrderRepository _ordersRepository;

        public OrderMutations(
            IOrderRepository orderRepository)
        {
            _ordersRepository = orderRepository;
        }

        [Authorize(Roles = new[] { ConstantsAPI.USERS })]
        public async Task<Order> CreateOrder(List<string> basketItemIds,
            string transactionId,
            string creditCardId,
            string addressId,
            [Service] UserManager<UserAuthAccess> userManager,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal currentUser)
        {
            var id = currentUser.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            return await _ordersRepository.CreateOrder(basketItemIds, transactionId, creditCardId, addressId, id);
        }

        [Authorize(Roles = new[] { ConstantsAPI.USERS })]
        public async Task<IEnumerable<Order>> RemoveOrder(string orderId,
            [Service] UserManager<UserAuthAccess> userManager,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal currentUser)
        {
            var id = currentUser.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            var order = await _ordersRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found!");
            }

            if (order.UserId != id)
            {
                throw new Exception("Access to resource denied.!");
            }

            await _ordersRepository.DeleteAsync(order);
            var spec = new OrderSpecification(userId: id);
            return await _ordersRepository.ListAsync(spec);
        }
    }
}
