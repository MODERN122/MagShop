﻿
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Infrastructure.Identity
{
    public class UserAuthAccess : IdentityUser
    {
        public UserAuthAccess(string userName):base(userName)
        {
            Email = userName;
        }
        public string FirebaseToken { get; set; }
        public string FacebookToken { get; set; }
        public string GoogleToken { get; set; }
        public string OauthToken { get; set; }
        public DateTime LastDatetimeAuth { get; set; } = DateTime.Now;
    }
}