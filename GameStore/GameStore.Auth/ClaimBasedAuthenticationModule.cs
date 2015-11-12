
using System;
using System.Security.Claims;
using System.Web;

namespace GameStore.Auth
{
    public class ClaimBasedAuthenticationModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += ReplacePrincipal;
        }

        public void Dispose()
        {
            
        }

        private static void ReplacePrincipal(object sender, EventArgs e)
        {
            if ((HttpContext.Current.User.Identity as ClaimsIdentity).
            {
                
            }
        }
    }
}
