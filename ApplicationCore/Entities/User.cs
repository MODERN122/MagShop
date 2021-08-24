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
    public class User : IAggregateRoot
    {
        public User()
        {

        }
        public User(string id,
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            DateTimeOffset birthDate,
            Basket basket,
            List<CreditCard> creditCards,
            List<Address> addresses)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            _cards = creditCards;
            _addresses = addresses;
            Basket = basket;
            BirthDate = birthDate;
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        //Collection product id favorites
        public string[] FavoriteProductsId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        private readonly List<CreditCard> _cards = new List<CreditCard>();
        public IReadOnlyCollection<CreditCard> CreditCards => _cards.AsReadOnly();

        private readonly List<Order> _orders = new List<Order>();
        public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();
        private readonly List<Address> _addresses = new List<Address>();
        public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();
        public DateTimeOffset BirthDate { get; set; } = DateTime.Now;
        public Basket Basket { get; set; }
        [JsonIgnore]
        public List<Store> Stores { get; set; }

        public void AddCreditCard(CreditCard creditCard)
        {
            if (!CreditCards.Any(i => i.CreditCardId == creditCard.CreditCardId))
            {
                _cards.Add(creditCard);
                return;
            }
        }
        public void AddOrder(Order order)
        {
            if (!Orders.Any(i => i.Id == order.Id))
            {
                _orders.Add(order);
                return;
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
        public string AddressId { get; set; } = Guid.NewGuid().ToString();
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Apartment { get; set; }
        public string ZipCode { get; set; }
    }


    public class CreditCard
    {
        public CreditCard()
        {
        }
        public string CreditCardId { get; set; }
        [DataType(DataType.CreditCard)]
        public string CardNumber { get; set; }
    }
}
