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
        public ClaimsAuthorizeApiAttribute(string type, string value)
        {
            claimType = type;
            claimValue = value;

        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var user = HttpContext.Current.User as ClaimsPrincipal;
            if (user != null && (user.HasClaim(claimType, claimValue) || user.HasClaim(claimType, Permissions.Full)))
            {
                return;
            }
            HandleUnauthorizedRequest(actionContext);
        }



    }
}