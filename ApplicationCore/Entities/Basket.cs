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
        public List<BasketItem> Items { get; private set; } = new List<BasketItem>();

        public void AddBasketItem(BasketItem basketItem)
        {
            Guard.Against.Null(basketItem, nameof(Items));
            var itemDublicates = Items.Where(x => x.ProductId == basketItem.ProductId).ToList();
            if (itemDublicates.Count>0)
            {
                var itemDublicate = itemDublicates.FirstOrDefault(x => x.SelectedProductPropertyItemIds.All(x => basketItem.SelectedProductPropertyItemIds.Contains(x)));
                if (itemDublicate!=null)
                {
                    var index = Items.IndexOf(itemDublicate);

                    if (index < 0)
                    {
                        throw new Exception("Cant get index of item dublicate!");
                    }

                    Items[index].AddQuantity(1);
                }
                else
                {
                    //Items.Remove(itemDublicate);
                    Items.Add(basketItem);
                }
            }
            else
            {
                Items.Add(basketItem);
            }
        }
        public void SubstractBasketItem(BasketItem basketItem)
        {

            Guard.Against.Null(basketItem, nameof(Items));
            var itemDublicate = Items.Find(x => x.Id == basketItem.Id);
            if (itemDublicate != null)
            {
                itemDublicate.SubstractQuantity(1);
            }
        }
        public void RemoveBasketItem(BasketItem basketItem)
        {
            Guard.Against.Null(basketItem, nameof(Items));
            var item = Items.Find(x => x.Id == basketItem.Id);
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
