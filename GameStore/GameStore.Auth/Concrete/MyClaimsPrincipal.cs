using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Auth.Concrete
{
    public sealed class MyClaimsPrincipal : ClaimsPrincipal
    {
        public MyClaimsPrincipal(String userName, IEnumerable<Claim> claims)
        {
            var genericIdentity = new GenericIdentity(userName, "Local auth");
            var claimsIdentity = new ClaimsIdentity(genericIdentity, claims);
            AddIdentity(claimsIdentity);
        }
    }
}
