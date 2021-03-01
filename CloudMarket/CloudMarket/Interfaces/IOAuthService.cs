using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CloudMarket.Interfaces
{
    public interface IOAuthService
    {
        Task<LoginResult> Login();
        void Logout();
    }

    public class LoginResult
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
