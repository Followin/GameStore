using System;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using GameStore.Auth.Abstract;
using GameStore.Auth.Models;
using GameStore.Auth.Utils;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;

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
                    PasswordHash = Crypto.HashPassword(userModel.Password)
                },
                userModel.Roles.Split(',')
                         .Select(role => new UserClaim() {Type = ClaimTypes.Role, Value = role, Issuer = "GameStore"})
                );
            _db.Save();
        }

        public void Login(string name, string password, Boolean isPersistent)
        {
            var user = _db.Users.GetSingle(
                x => x.Name == name && 
                     Crypto.VerifyHashedPassword(x.PasswordHash, password));
            
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            var claims = user.Claims.Select(x => new Claim(x.Type, x.Value, null, x.Issuer));
            ClaimsPrincipal principal = new MyClaimsPrincipal(user.Name, claims);

            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddHours(24),
            };
            var ticket = new AuthenticationTicket(principal.Identity as ClaimsIdentity, authenticationProperties);

            var ticketDataFormat = new TicketDataFormat(new MachineKeyProtector(AuthPurpose));
            var serializedTicket = ticketDataFormat.Protect(ticket);

            var newCookie = new HttpCookie(CookieName, serializedTicket);
            HttpContext.Current.Response.Cookies.Add(newCookie);

        }

        public void Logout()
        {
            var cookie = HttpContext.Current.Response.Cookies[CookieName];
            if (cookie != null)
            {
                cookie.Value = String.Empty;
            }
        }
    }
}
