
using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using GameStore.Auth.Concrete;
using GameStore.Auth.Utils;
using GameStore.Static;
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
                var ticketDataFormat = new TicketDataFormat(new MachineKeyProtector(AuthenticationService.AuthPurpose));
                var ticket = ticketDataFormat.Unprotect(cookie.Value);
                if (ticket != null)
                {
                    var claimsPrincipal = new ClaimsPrincipal(ticket.Identity);
                    HttpContext.Current.User = claimsPrincipal;
                }
                else
                {
                    var claimsPrincipal = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            RoleClaims.GetClaimsForRole(Roles.Guest)
                                      .Concat(new[] {new Claim(ClaimTypes.Role, Roles.Guest)})));
                    HttpContext.Current.User = claimsPrincipal;
                }
            }
        }
    }
}
