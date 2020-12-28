using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationCore.Entities
{
    public class UserAuthAccess
    {

        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string UserId { get; set; }
        public string FirebaseToken { get; set; }
        public string FacebookToken { get; set; }
        public string GoogleToken { get; set; }
        public string OauthToken { get; set; }
        public DateTime LastDatetimeAuth { get; set; } = DateTime.Now;

    }
}
