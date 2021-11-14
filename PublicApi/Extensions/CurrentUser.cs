using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PublicApi.Extensions
{
    public class CurrentUser
    {
        public ClaimsPrincipal User { get; }
        public string UserId { get; }

        public CurrentUser(ClaimsPrincipal user)
        {
            User = user;
            UserId = user.Claims.First().Value;
        }
    }

    public class CurrentUserGlobalState : GlobalStateAttribute
    {
        public CurrentUserGlobalState() : base("currentUser")
        {
        }
    }
}
