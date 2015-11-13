﻿using System;
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
                    PasswordHash = Crypto.HashPassword(userModel.Password)
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

            var claims = user.Claims.Select(x => new Claim(x.Type, x.Value, null, x.Issuer)).ToList();
            claims.AddRange(claims.Where(x => x.Type == ClaimTypes.Role).ToList().SelectMany(x => RoleClaims.GetClaimsForRole(x.Value)));
            claims.Add(new Claim(ClaimTypes.SerialNumber, user.Id.ToString()));
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

            return new LoginResult { Status = LoginResultStatus.Success };
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