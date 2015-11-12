using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.DAL.EF;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class UserRepository : GenericRepository<User, Int32>, IUserRepository
    {
        public UserRepository(EFContext context) : base(context)
        {
        }

        public void AddUserWithClaims(User user, IEnumerable<UserClaim> claims)
        {
            throw new NotImplementedException();
        }

        public void AddClaim(UserClaim claim)
        {
            Db.UserClaims.Add(new UserClaim());
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