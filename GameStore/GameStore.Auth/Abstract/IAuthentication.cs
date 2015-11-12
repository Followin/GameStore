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

        LoginResult Login(String name, String password, Boolean isPersistent);

        void Logout();
    }
}
