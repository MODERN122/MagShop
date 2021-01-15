using PublicApi.Base;

namespace PublicApi.Endpoints.Authentication
{
    public class AuthenticationRequest : BaseRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}