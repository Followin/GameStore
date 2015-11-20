using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Auth.Abstract
{
    public interface IUserService
    {
        bool IsUsernameFree(string name);

        void BanUser(int userId, DateTime expirationTime);

        IEnumerable<Claim> GetUserClaims(int id);
    }
}
