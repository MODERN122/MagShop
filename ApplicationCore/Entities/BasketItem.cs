using Ardalis.GuardClauses;
using System;

namespace ApplicationCore.Entities
{
    public class BasketItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
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
            ProductId = product.Id;
            SetQuantity(quantity);
        }

        public void AddQuantity(int quantity)
        {
            Guard.Against.OutOfRange(quantity, nameof(quantity), 1, int.MaxValue);
            if (Quantity + quantity <= 999)
            {
                Quantity += quantity;
            }
            else
            {
                Quantity = 999;
            }
        }
        public void SubstractQuantity(int quantity)
        {
            Guard.Against.OutOfRange(quantity, nameof(quantity), 1, int.MaxValue);
            if (Quantity > quantity)
            {
                Quantity -= quantity;
            }
            else
            {
                Quantity = 1;
            }
            
        }

        public void SetQuantity(int quantity)
        {
            Guard.Against.OutOfRange(quantity, nameof(quantity), 1, int.MaxValue);
            Quantity = quantity;
        }
    }
}