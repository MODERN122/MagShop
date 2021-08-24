using Ardalis.GuardClauses;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities
{
    public class Basket : IAggregateRoot
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        private readonly List<BasketItem> _items = new List<BasketItem>();
        public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();

        public void AddBasketItem(BasketItem item)
        {
            Guard.Against.Null(item, nameof(_items));
            var itemDublicate = _items.Find(x => x.ProductId == item.ProductId);
            if (itemDublicate!=null)
            {
                itemDublicate.AddQuantity(item.Quantity);
            }
            else
            {
                _items.Add(item);
            }
        }
        public void SetBasketItems(List<BasketItem> basketItems)
        {
            Guard.Against.NullOrEmpty(basketItems, nameof(_items));
            _items.Clear();
            _items.AddRange(basketItems);
        }
        public Basket()
        {

        }
    }
}
