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
        Boolean IsUsernameFree(String name);

        void BanUser(Int32 userId, DateTime expirationTime);

        IEnumerable<Claim> GetUserClaims(Int32 id);
    }
}
