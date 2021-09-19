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

        public void AddBasketItem(BasketItem basketItem)
        {
            Guard.Against.Null(basketItem, nameof(_items));
            var itemDublicate = _items.Find(x => x.ProductId == basketItem.ProductId);
            if (itemDublicate != null)
            {
                itemDublicate.AddQuantity(1);
            }
            else
            {
                _items.Add(basketItem);
            }
        }
        public void RemoveBasketItem(BasketItem basketItem)
        {
            Guard.Against.Null(basketItem, nameof(_items));
            var item = _items.Find(x => x.ProductId == basketItem.ProductId);
            _items.Remove(item);
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
