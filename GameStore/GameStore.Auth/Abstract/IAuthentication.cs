using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Auth.Models;

namespace GameStore.Auth.Abstract
{
    public interface IAuthenticationService
    {
        void Register(RegisterUserModel userModel);

        LoginResult Login(string name, string password, bool isPersistent);

        void Logout();
    }
}
