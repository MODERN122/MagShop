using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Order
    {
        public Order()
        {

        }
        public Order(DateTime datetimeOrder, string addressId, List<OrderItem> items)
        {
            DateTimeOrder = datetimeOrder;
            AddressId = addressId;
            AddRangeOrderItems(items);
        }
        
        public string OrderId { get; set; } = Guid.NewGuid().ToString();

        private readonly List<OrderItem> _items = new List<OrderItem>();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
        public DateTime DateTimeOrder { get; set; } = DateTime.Now;
        public string AddressId { get; set; }
        public Address ShipToAddress { get; set; }
        public double Total()
        {
            var total = 0.0;
            foreach (var item in _items)
            {
                total +=Math.Round(item.UnitPrice * item.Quantity,2);
            }
            return total;
        }
        public void AddRangeOrderItems(List<OrderItem> orderItems)
        {
            Guard.Against.NullOrEmpty(orderItems, nameof(_items));
            _items.AddRange(orderItems);
        }
    }

    public class OrderItem
    {
        public OrderItem()
        {

        }
        public OrderItem(int quantity, Product product)
        {
            UnitPrice = product.PriceNew;
            ProducOrdered = product;
            SetQuantity(quantity);
        }
        public string OrderItemId { get; set; } = Guid.NewGuid().ToString();
        public double UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public string ProductId { get; set; }
        public Product ProducOrdered { get; set; }

        public void SetQuantity(int quantity)
        {
            Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

            Quantity = quantity;
        }
    }
}
