
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Infrastructure.Identity
{
    public class UserAuthAccess : IdentityUser
    {
        public UserAuthAccess(string userName, string email)
        {
            UserName = userName;
            Email = email;
        }
        public override string UserName { get; set; }
        public string FirebaseToken { get; set; }
        public string FacebookToken { get; set; }
        public string GoogleToken { get; set; }
        public string OauthToken { get; set; }
        public DateTime LastDatetimeAuth { get; set; } = DateTime.Now;
    }
}
