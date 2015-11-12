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
        private List<Claim> _claims; 
        public MyClaimsPrincipal(String userName, IEnumerable<Claim> claims)
        {
            _claims = new List<Claim> {new Claim(ClaimTypes.Name, userName)};
            _claims.AddRange(claims);

            var claimsIdentity = new ClaimsIdentity(_claims, "Local claims auth", ClaimTypes.Name, ClaimTypes.Role);

            AddIdentity(claimsIdentity);
            
        }
    }
}
