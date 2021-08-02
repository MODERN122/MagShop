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

        public CurrentUser(ClaimsPrincipal user)
        {
            User = user;
        }
    }

    public class CurrentUserGlobalState : GlobalStateAttribute
    {
        public CurrentUserGlobalState() : base("currentUser")
        {
        }
    }
}
