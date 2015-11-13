using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using GameStore.Static;

namespace GameStore.Web.Filters
{
    public class ClaimsAuthorizeAttribute : AuthorizeAttribute
    {
        private string claimType;
        private string claimValue;
        public ClaimsAuthorizeAttribute(string type, string value)
        {
            claimType = type;
            claimValue = value;

        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = HttpContext.Current.User as ClaimsPrincipal;
            if (user != null && (user.HasClaim(claimType, claimValue) || user.HasClaim(claimType, Permissions.Full)))
            {
                return;
            }
            HandleUnauthorizedRequest(filterContext);
        }

    }
}