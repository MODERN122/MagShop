using ApplicationCore.Base;

namespace ApplicationCore.RESTApi.Authentication
{
    public class AuthenticationRequest : BaseRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}