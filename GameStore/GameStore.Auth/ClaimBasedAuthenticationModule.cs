
using System;
using System.Security.Claims;
using System.Web;
using GameStore.Auth.Concrete;
using GameStore.Auth.Utils;
using Microsoft.Owin.Security.DataHandler;

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
            var cookie = HttpContext.Current.Request.Cookies[AuthenticationService.CookieName];
            if (cookie != null)
            {
                var ticketDataFormat = new TicketDataFormat(new MachineKeyProtector());
                var ticket = ticketDataFormat.Unprotect(cookie.Value);

                var claimsPrincipal = new ClaimsPrincipal(ticket.Identity);
                HttpContext.Current.User = claimsPrincipal;
            }
        }
    }
}
