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
        public List<BasketItem> Items { get; private set; }

        public void AddBasketItem(BasketItem basketItem)
        {
            Guard.Against.Null(basketItem, nameof(Items));
            var itemDublicate = Items.Find(x => x.ProductId == basketItem.ProductId);
            if (itemDublicate != null)
            {
                itemDublicate.AddQuantity(1);
            }
            else
            {
                Items.Add(basketItem);
            }
        }
        public void RemoveBasketItem(BasketItem basketItem)
        {
            Guard.Against.Null(basketItem, nameof(Items));
            var item = Items.Find(x => x.ProductId == basketItem.ProductId);
            Items.Remove(item);
        }
        public void SetBasketItems(List<BasketItem> basketItems)
        {
            Guard.Against.NullOrEmpty(basketItems, nameof(Items));
            Items.Clear();
            Items.AddRange(basketItems);
        }
        public Basket()
        {

        }
    }
}
