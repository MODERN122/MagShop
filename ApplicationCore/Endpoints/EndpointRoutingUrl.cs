using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Endpoints
{
    public static class EndpointRoutingUrl
    {
        public const string USERS_URL = "api/users";
        public const string BASKET_URL = "api/users/{id}/basket_items";
        public const string AUTHENTICATION_URL = "api/authentication";
        public const string PRODUCTS_URL = "api/products";
    }
}
