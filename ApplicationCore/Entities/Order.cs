using ApplicationCore.Interfaces;
using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Order : BaseDateTimeEntity
    {
        [Obsolete("Uses only for EF Core generating")]
        public Order() { }
        public Order(DateTime publicationDateTime, string addressId, List<OrderItem> items, string userId):base(userId)
        {
            PublicationDateTime = publicationDateTime;
            AddressId = addressId;
            AddRangeOrderItems(items);
        }
        
        public string Id { get; set; } = Guid.NewGuid().ToString();

        private readonly List<OrderItem> _items = new List<OrderItem>();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
        public string AddressId { get; set; }
        public Address ShipToAddress { get; set; }
        public double Total()
        {
            var total = 0.0;
            foreach (var item in _items)
            {
                total +=Math.Round(item.UnitPrice.Value * item.Quantity,2);
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
        public OrderItem(int quantity, Product product, List<ProductPropertyItem> propertyItems)
        {
            UnitPrice = propertyItems.First(x=>x.PriceNew!=null).PriceNew;
            Product = product;
            SetQuantity(quantity);
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public double? UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }

        public void SetQuantity(int quantity)
        {
            Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

            Quantity = quantity;
        }
    }
}
