using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
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

}
