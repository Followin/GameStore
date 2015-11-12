using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;
using GameStore.Auth.Abstract;
using GameStore.Auth.Models;
using GameStore.Auth.Utils;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataHandler.Serializer;

namespace GameStore.Auth.Concrete
{
    public class AuthenticationService : IAuthenticationService
    {
        private IGameStoreUnitOfWork _db;
        internal const String CookieName = "AUTHENTICATION";

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
                    PasswordHash = userModel.Password
                },
                userModel.Roles.Split(',')
                         .Select(role => new UserClaim() {Type = ClaimTypes.Role, Value = role, Issuer = "GameStore"})
                );
            _db.Save();
        }

        public void Login(string name, string password, Boolean isPersistent)
        {
            var user = _db.Users.GetSingle(x => x.Name == name && x.PasswordHash == password);
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

            var ticketDataFormat = new TicketDataFormat(new MachineKeyProtector());
            var serializedTicket = ticketDataFormat.Protect(ticket);

            var newCookie = new HttpCookie(CookieName, serializedTicket);
            HttpContext.Current.Response.Cookies.Add(newCookie);

        }

        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}
