using System;
using GameStore.DAL.Abstract;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class UserRepository : GenericRepository<User, Int32>, IUserRepository
    {
        public UserRepository(IContext context) : base(context)
        {
        }
    }
}