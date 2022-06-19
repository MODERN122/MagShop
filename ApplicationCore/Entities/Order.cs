using ApplicationCore.Interfaces;
using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Order : BaseDateTimeEntity
    {
        [Obsolete("Uses only for EF Core generating")]
        public Order() { }
        public Order(string addressId, List<OrderItem> items, string userId, string transactionId, string creditCardId) : base(userId)
        {
            TransactionId = transactionId;
            UserId = userId;
            AddressId = addressId;
            AddRangeOrderItems(items);
            TotalPrice = Total();
            CreditCardId = creditCardId;            
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public double TotalPrice { get; set; }
        public User User { get; set; }
        public string CreditCardId { get; set; }
        public CreditCard CreditCard { get; set; }
        public string TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        public List<OrderItem> Items { get; private set; } = new List<OrderItem>() { };
        public string AddressId { get; set; }
        public Address ShipToAddress { get; set; }
        public double Total()
        {
            var total = 0.0;
            foreach (var item in Items)
            {
                total += Math.Round(item.TotalPrice, 2);
            }
            return total;
        }
        public void AddRangeOrderItems(List<OrderItem> orderItems)
        {
            Guard.Against.NullOrEmpty(orderItems, nameof(Items));
            Items.AddRange(orderItems);
        }
    }

    public class OrderItem
    {
        public OrderItem()
        {

        }
        public OrderItem(int quantity, Product product, List<ProductPropertyItem> selectedPropertyItems)
        {
            UnitPrice = selectedPropertyItems.Select(x => x.PriceNew).Sum();
            SelectedProductPropertyItemIds = selectedPropertyItems.Select(x => x.Id).ToList();
            ProductId = product.Id;
            SetQuantity(quantity);
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string OrderId { get; set; }
        public double UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public string ProductId { get; private set; }
        public List<string> SelectedProductPropertyItemIds { get; set; } = new List<string>();
        [NotMapped]
        public double TotalPrice { set { } get { return UnitPrice * Quantity; } }
        public Product Product { get; set; }
        public void SetQuantity(int quantity)
        {
            Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

            Quantity = quantity;
        }
    }
}
