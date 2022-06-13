using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationCore.GraphQLEndpoints
{
    public class UserPayload
    {
        public UserPayload(User user, Token token)
        {
            User = user;
            Token = token;
        }

        public User User { get; }
        public Token Token { get; }
    }
    public class RegisterSellerPayload
    {
        public RegisterSellerPayload(Seller seller, Token token)
        {
            Seller = seller;
            Token = token;
        }

        public Seller Seller { get; }
        public Token Token { get; }
    }

    public class Token
    {
        public string AccessToken { get; set;}
        public string RefreshToken { get; set; }
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
