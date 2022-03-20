using ApplicationCore.Interfaces;
using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace ApplicationCore.Entities
{

    public class User : BaseDateTimeEntity
    {
        [Obsolete("Uses only for EF Core generating")]
        public User() { }
        public User(string id) : base(id) { }
        public User(string id,
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            DateTime birthDate,
            Basket basket,
            List<CreditCard> creditCards,
            List<Address> addresses) : base(id)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            CreditCards = creditCards;
            Addresses = addresses;
            Basket = basket;
            BirthDate = birthDate.ToUniversalTime();
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        //Collection product id favorites
        public List<string> FavoriteProductsId { get; private set; } = new List<string>();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public List<CreditCard> CreditCards { get; private set; }
        public List<Order> Orders { get; private set; }
        public List<Address> Addresses { get; private set; }
        public DateTime? BirthDate { get; set; }
        public Basket Basket { get; set; }
        [JsonIgnore]
        public List<Store> Stores { get; set; }

        //public void AddCreditCard(CreditCard creditCard)
        //{
        //    if (!CreditCards.Any(i => i.CreditCardId == creditCard.CreditCardId))
        //    {
        //        _cards.Add(creditCard);
        //        return;
        //    }
        //}
        //public void AddOrder(Order order)
        //{
        //    if (!Orders.Any(i => i.Id == order.Id))
        //    {
        //        _orders.Add(order);
        //        return;
        //    }
        //}
        public void AddItemToBasket(BasketItem basketItem)
        {
            Basket.AddBasketItem(basketItem);
        }
        public void SubstractItemFromBasket(BasketItem basketItem)
        {
            Basket.SubstractBasketItem(basketItem);
        }
        public void RemoveItemFromBasket(BasketItem basketItem)
        {
            Basket.RemoveBasketItem(basketItem);
        }
        public bool AddProductToFavorite(string productId)
        {
            if (this.FavoriteProductsId.Contains(productId))
            {
                return false;
            }
            else
            {
                this.FavoriteProductsId.Add(productId);
                return true;
            }
        }
        public bool RemoveProductFromFavorite(string productId)
        {
            if (!this.FavoriteProductsId.Contains(productId))
            {
                return false;
            }
            else
            {
                this.FavoriteProductsId.Remove(productId);
                return true;
            }
        }

        public void AddItemsToBasket(List<BasketItem> basketItems)
        {
            var nowItems = Basket.Items.ToList();
            nowItems.AddRange(basketItems);
            List<BasketItem> resultSequence = new List<BasketItem>();
            var groupCollection = nowItems.GroupBy(x => x.ProductId).ToList();
            groupCollection.ForEach(x =>
            {
                if (x.Count() > 1)
                {
                    x.Skip(1).ToList().ForEach(y => x.First().AddQuantity(y.Quantity));
                    resultSequence.Add(x.First());
                }
                else
                {
                    resultSequence.Add(x.First());
                }
            });
            Basket.SetBasketItems(resultSequence);
        }
    }

    public class Address
    {
        public Address()
        {

        }
        public Address(string street, string apartment)
        {
            Street = street;
            Apartment = apartment;
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool IsDefault { get; set; } 
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Housing { get; set; }
        public string Apartment { get; set; }
        public int? Floor { get; set; }
        public string ZipCode { get; set; }
        public string Title { get;set; }
    }


    public class CreditCard
    {
        public CreditCard()
        {
        }
        public string Id { get; set; }
        [DataType(DataType.CreditCard)]
        public string CardNumber { get; set; }
        public bool IsDefault { get; set; }

    }
}
