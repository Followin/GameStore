using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Web.Helpers;
using GameStore.Auth.Abstract;
using GameStore.Auth.Models;
using GameStore.DAL.Abstract;
using GameStore.DAL.EF;
using GameStore.Domain.Entities;
using GameStore.Static;

namespace GameStore.Auth.Concrete
{
    public class UserService : IUserService
    {
        private IGameStoreUnitOfWork _db;

        public UserService(IGameStoreUnitOfWork db)
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
                         .Select(claim => new UserClaim() { Type = claim.Type, Value = claim.Value, Issuer = "GameStore" })
                );
            _db.Save();
        }

        public bool IsUsernameFree(string name)
        {
            return _db.Users.GetFirst(x => x.Name == name) == null;
        }

        public void BanUser(int userId, DateTime expirationTime)
        {
            var user = _db.Users.Get(userId);
            if (user == null)
            {
                throw new ArgumentOutOfRangeException("userId", "user not found");
            }

            user.BanExpirationTime = expirationTime;
            _db.Users.Update(user);
            _db.Save();
        }

        public IEnumerable<Claim> GetUserClaims(int id)
        {
            var user = _db.Users.Get(id);

            if (user == null)
            {
                throw new ArgumentOutOfRangeException("id", "User not found");
            }

            var claims = user.Claims.Select(x => new Claim(x.Type, x.Value, null, x.Issuer)).ToList();
            claims.AddRange(claims.Where(x => x.Type == ClaimTypes.Role).ToList().SelectMany(x => RoleClaims.GetClaimsForRole(x.Value)));
            claims.Add(new Claim(ClaimTypes.Name, user.Name));
            claims.Add(new Claim(ClaimTypes.SerialNumber, user.Id.ToString()));

            if (user.BanExpirationTime.HasValue && user.BanExpirationTime > DateTime.UtcNow)
            {
                var claim =
                    claims.FirstOrDefault(
                        x => x.Type == ClaimTypesExtensions.CommentPermission && x.Value == Permissions.Add);
                if (claim != null)
                {
                    claims.Remove(claim);
                }
            }

            return claims;
        }
    }
}
