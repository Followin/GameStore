using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.DAL.Abstract;
using GameStore.DAL.Abstract.Repositories;
using GameStore.DAL.EF;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class UserRepository : GenericRepository<User, int>, IUserRepository
    {
        public UserRepository(IEFContext context) : base(context)
        {
        }

        public void AddUserWithClaims(User user, IEnumerable<UserClaim> claims)
        {
            var addedUser = Db.Users.Add(user);
            var bindedClaims = claims.Select(x => new UserClaim()
            {
                Issuer = x.Issuer,
                Value = x.Value,
                User = addedUser,
                Type = x.Type
            });
            AddClaims(bindedClaims);


        }

        public void AddClaim(UserClaim claim)
        {
            Db.UserClaims.Add(claim);
        }

        public void AddClaims(IEnumerable<UserClaim> claims)
        {
            foreach (var userClaim in claims)
            {
                AddClaim(userClaim);
            }
        }

        public void DeleteClaim(UserClaim claim)
        {
            Db.UserClaims.Remove(claim);
        }

        public void DeleteClaims(IEnumerable<UserClaim> claims)
        {
            foreach (var claim in claims)
            {
                DeleteClaim(claim);
            }
        }

        public IEnumerable<UserClaim> GetClaims(int userId)
        {
            return Db.Users.Find(userId).Claims.ToList();
        }
    }
}