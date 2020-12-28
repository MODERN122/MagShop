using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApplicationCore.Entities
{
    public class User
    {
        public User()
        {

        }
        [Key]
        public string Id { get; set; }
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
        public DateTime BirthDate { get; set; } = DateTime.Now;
        public Basket Basket { get; set; }

        public UserAuthAccess UserAuthAccess { get; set; }

    }

    public class Address
    {
        public Address()
        {

        }
        public string AddressId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Apartment { get; set; }
        public string ZipCode { get; set; }
    }


    public class CreditCard
    {
        public CreditCard() {
        }
        public string CreditCardId { get; set; }
        [DataType(DataType.CreditCard)]
        public string CardNumber { get; set; }
    }
}
