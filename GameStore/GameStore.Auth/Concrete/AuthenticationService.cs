using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using GameStore.Auth.Abstract;
using GameStore.Auth.Models;
using GameStore.Auth.Utils;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Static;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;

namespace GameStore.Auth.Concrete
{
    public class AuthenticationService : IAuthenticationService
    {
        private IGameStoreUnitOfWork _db;
        internal const String CookieName = "AUTHENTICATION";
        internal const String AuthPurpose = "Auth";

        public AuthenticationService(IGameStoreUnitOfWork db)
        {
            _db = db;
        }

        public void Register(RegisterUserModel userModel)
        {
            
            _db.Users.AddUserWithClaims(
                new User
                {
                    Name = userModel.Name,
                    PasswordHash = Crypto.HashPassword(userModel.Password),
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                userModel.Claims
                         .Select(claim => new UserClaim() {Type = claim.Type, Value = claim.Value, Issuer = "GameStore"})
                );
            _db.Save();
        }

        public LoginResult Login(string name, string password, Boolean isPersistent)
        {
            var user = _db.Users.GetSingle(
                x => x.Name == name && 
                     Crypto.VerifyHashedPassword(x.PasswordHash, password));
            
            if (user == null)
            {
                return new LoginResult { Status = LoginResultStatus.WrongCredentials };
            }


            var claims = new[] {new Claim(ClaimTypes.SerialNumber, user.Id.ToString())};
            ClaimsPrincipal principal = new MyClaimsPrincipal(claims);

            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddHours(24)
            };
            authenticationProperties.Dictionary.Add("Stamp", user.SecurityStamp);
            var ticket = new AuthenticationTicket(principal.Identity as ClaimsIdentity, authenticationProperties);

            var ticketDataFormat = new TicketDataFormat(new MachineKeyProtector(AuthPurpose));
            var serializedTicket = ticketDataFormat.Protect(ticket);

            var newCookie = new HttpCookie(CookieName, serializedTicket);
            HttpContext.Current.Response.Cookies.Add(newCookie);

            return new LoginResult { Status = LoginResultStatus.Success };
        }

        public void Logout()
        {
            var cookie = HttpContext.Current.Request.Cookies[CookieName];
            if (cookie != null)
            {
                var ticketDataFormat = new TicketDataFormat(new MachineKeyProtector(AuthPurpose));
                var ticket = ticketDataFormat.Unprotect(cookie.Value);
                if (ticket != null)
                {
                    var idClaim = ticket.Identity.FindFirst(ClaimTypes.SerialNumber);
                    var id = Int32.Parse(idClaim.Value);
                    var user = _db.Users.Get(id);
                    if (user != null)
                    {
                        user.SecurityStamp = Guid.NewGuid().ToString();
                        _db.Users.Update(user);
                        _db.Save();
                    }
                }
                cookie = HttpContext.Current.Response.Cookies[CookieName];
                cookie.Value = String.Empty;
            }
        }
    }
}
