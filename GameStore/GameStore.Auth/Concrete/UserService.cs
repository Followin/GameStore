using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Auth.Abstract;
using GameStore.Domain.Abstract;

namespace GameStore.Auth.Concrete
{
    public class UserService : IUserService
    {
        private IGameStoreUnitOfWork _db;

        public UserService(IGameStoreUnitOfWork db)
        {
            _db = db;
        }

        public Boolean IsUsernameFree(string name)
        {
            return _db.Users.GetSingle(x => x.Name == name) == null;
        }
    }
}
