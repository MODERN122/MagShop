using Ardalis.GuardClauses;

namespace ApplicationCore.Entities
{
    public class BasketItem
    {
        public string BasketItemId { get; set; }
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public string ProductId { get; private set; }
        public string BasketId { get; private set; }

        public BasketItem()
        {

        }
        public BasketItem(string productId, int quantity, decimal unitPrice)
        {
            ProductId = productId;
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