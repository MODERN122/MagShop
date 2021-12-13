
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Infrastructure.Identity
{
    public class UserAuthAccess : IdentityUser
    {
        public UserAuthAccess(string userName) : base(userName)
        {
            Id = userName;
        }
        public UserAuthAccess(string userName, string phoneNumber) : base(userName)
        {
            PhoneNumber = phoneNumber;
        }
        public UserAuthAccess(string userName, string email, string some) : base(userName)
        {
            Email = email;
        }
        public string FirebaseToken { get; set; }
        public string FacebookToken { get; set; }
        public string GoogleToken { get; set; }
        public string OauthToken { get; set; }
        public DateTime? LastDatetimeAuth { get; set; } = DateTime.UtcNow;
    }
}
