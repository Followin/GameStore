using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GameStore.Auth.Models;

namespace GameStore.Auth.Abstract
{
    public interface IUserService
    {
        void Register(RegisterUserModel userModel);

        bool IsUsernameFree(string name);

        void BanUser(int userId, DateTime expirationTime);

        IEnumerable<Claim> GetUserClaims(int id);
    }
}
