using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using GameStore.Auth.Abstract;
using GameStore.Auth.Concrete;
using GameStore.Auth.Utils;
using GameStore.DAL.Abstract;
using GameStore.Static;
using Microsoft.Owin.Security.DataHandler;

namespace GameStore.Auth
{
    public class ClaimBasedAuthenticationModule : IHttpModule
    {
        private DependencyInjector _injector;

        public delegate object DependencyInjector(Type type);

        public ClaimBasedAuthenticationModule(DependencyInjector injector)
        {
            _injector = injector;
        }

        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += ReplacePrincipal;
        }

        public void Dispose()
        {

        }

        private void ReplacePrincipal(object sender, EventArgs e)
        {
            var cookie = HttpContext.Current.Request.Cookies[AuthenticationService.CookieName];
            if (cookie != null)
            {
                var ticketDataFormat = new TicketDataFormat(new MachineKeyProtector(AuthenticationService.AuthPurpose));
                var ticket = ticketDataFormat.Unprotect(cookie.Value);
                if (ticket != null)
                {
                    var idClaim = ticket.Identity.FindFirst(ClaimTypes.SerialNumber);
                    var id = Int32.Parse(idClaim.Value);
                    var stamp = ticket.Properties.Dictionary["Stamp"];

                    var unitOfWork = (IGameStoreUnitOfWork) _injector.Invoke(typeof (IGameStoreUnitOfWork));
                    var user =
                        unitOfWork.Users.Get(id);

                    if (user == null ||user.SecurityStamp != stamp)
                    {
                        LoginAsGuest();
                        return;
                    }

                    var userService = new UserService(unitOfWork);
                    ticket.Identity.AddClaims(userService.GetUserClaims(id));
                    var principal = new ClaimsPrincipal(ticket.Identity);

                    HttpContext.Current.User = principal;
                }
                else
                {
                    LoginAsGuest();
                }
            }
        }

        private void LoginAsGuest()
        {
            var claimsPrincipal = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            RoleClaims.GetClaimsForRole(Roles.Guest)
                                      .Concat(new[] { new Claim(ClaimTypes.Role, Roles.Guest) })));

            HttpContext.Current.User = claimsPrincipal;
        }
    }
}
