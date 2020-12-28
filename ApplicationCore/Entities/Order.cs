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
        public string OrderId { get; set; }
        public string UserId { get; set; }

        private readonly List<OrderItem> _items = new List<OrderItem>();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
        public DateTime DateTimeOrder { get; set; } = DateTime.Now;
        public string AddressId { get; set; }
        public Address ShipToAddress { get; set; }
        public decimal Total()
        {
            var total = 0m;
            foreach (var item in _items)
            {
                total += item.UnitPrice * item.Quantity;
            }
            return total;
        }

    }

    public class OrderItem
    {
        public OrderItem()
        {

        }
        public string OrderItemId { get; set; }
        public string OrderId { get; set; }
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public string ProductId { get; set; }
        public Product ProducOrdered { get; set; }

        public OrderItem(Product productPreview, int quantity, decimal unitPrice)
        {
            ProducOrdered = productPreview;
            UnitPrice = unitPrice;
            SetQuantity(quantity);
        }

        public void AddQuantity(int quantity)
        {
            Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

            Quantity += quantity;
        }

        public void SetQuantity(int quantity)
        {
            Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

            Quantity = quantity;
        }
    }
}
