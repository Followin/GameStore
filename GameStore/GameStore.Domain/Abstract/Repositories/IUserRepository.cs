using System;
using System.Collections;
using System.Collections.Generic;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Abstract.Repositories
{
    public interface IUserRepository : IRepository<User, Int32>
    {
        void AddUserWithClaims(User user, IEnumerable<UserClaim> claims);
        
        void AddClaim(UserClaim claim);

        void AddClaims(IEnumerable<UserClaim> claims);

        void DeleteClaim(UserClaim claim);

        void DeleteClaims(IEnumerable<UserClaim> claims);

        IEnumerable<UserClaim> GetClaims(Int32 userId);
    }
}