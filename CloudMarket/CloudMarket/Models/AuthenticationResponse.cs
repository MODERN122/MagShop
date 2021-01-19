using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMarket.Models
{
    public class AuthenticationResponse
    {
        public bool Result { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsNotAllowed { get; set; }
        public bool RequiresTwoFactor { get; set; }
    }
}
