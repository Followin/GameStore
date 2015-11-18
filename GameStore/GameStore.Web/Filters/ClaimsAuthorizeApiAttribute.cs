using System;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using GameStore.Static;

namespace GameStore.Web.Filters
{
    public class ClaimsAuthorizeApiAttribute : AuthorizeAttribute
    {
        private string claimType;
        private string claimValue;
        private Boolean _checkClaims = false;

        public ClaimsAuthorizeApiAttribute(string type, string value)
        {
            claimType = type;
            claimValue = value;
            _checkClaims = true;
        }

        public ClaimsAuthorizeApiAttribute()
        {
            
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var user = HttpContext.Current.User as ClaimsPrincipal;
            if (user == null)
            {
                HandleUnauthorizedRequest(actionContext);
            }

            if (_checkClaims && (!user.HasClaim(claimType, claimValue) && !user.HasClaim(claimType, Permissions.Full)))
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }
    }
}