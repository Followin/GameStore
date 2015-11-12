using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using GameStore.Auth.Abstract;
using GameStore.Auth.Models;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.Auth.Concrete
{
    public class AuthenticationService : IAuthenticationService
    {
        private IGameStoreUnitOfWork _db;

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

        public void Login(string name, string password)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}
