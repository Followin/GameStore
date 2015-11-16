using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using GameStore.Auth.Abstract;
using GameStore.Auth.Concrete;
using GameStore.Auth.Utils;
using GameStore.Domain.Abstract;
using GameStore.Static;
using Microsoft.Owin.Security.DataHandler;

namespace GameStore.Auth
{
    public class ClaimBasedAuthenticationModule : IHttpModule
    {
        private IUserService _userService;

        public delegate object DependencyInjector(Type type);

        public ClaimBasedAuthenticationModule(DependencyInjector injector)
        {
            var unitofwork = (IGameStoreUnitOfWork)injector.Invoke(typeof(IGameStoreUnitOfWork));
            _userService = new UserService(unitofwork);
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

                    ticket.Identity.AddClaims(_userService.GetUserClaims(id));
                    var principal = new ClaimsPrincipal(ticket.Identity);

                    HttpContext.Current.User = principal;
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
