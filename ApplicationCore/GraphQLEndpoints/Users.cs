using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationCore.GraphQLEndpoints
{
    public class RegisterUserPayload
    {
        public RegisterUserPayload(User user, string accessToken)
        {
            User = user;
            AccessToken = accessToken;
        }

        public User User { get; }
        public string AccessToken { get; }
    }
    public class RegisterSellerPayload
    {
        public RegisterSellerPayload(Seller seller, string accessToken)
        {
            Seller = seller;
            AccessToken = accessToken;
        }

        public Seller Seller { get; }
        public string AccessToken { get; }
    }
    public class EditUserInput
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get;set; }
        public DateTimeOffset? BirthDate { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
