using ApplicationCore.Entities;
using PublicApi.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PublicApi.Endpoints.Users
{
    public class CreateUserRequest : BaseRequest
    {
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public List<CreditCard> CreditCards { get; set; }
        public List<Address> Addresses { get; set; }
        [Required]
        public DateTimeOffset BirthDate { get; set; }
    }
}