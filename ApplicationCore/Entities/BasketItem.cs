using Ardalis.GuardClauses;
using System;

namespace ApplicationCore.Entities
{
    public class BasketItem
    {
        public string BasketItemId { get; set; } = Guid.NewGuid().ToString();
        public int Quantity { get; private set; }
        public string ProductId { get; private set; }
        public Product Product { get; set; }
        public string BasketId { get; private set; }

        public BasketItem()
        {

        }
        public BasketItem(int quantity, Product product)
        {
            Product = product;
            ProductId = product.ProductId;
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