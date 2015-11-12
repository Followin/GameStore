using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Auth.Abstract;
using GameStore.Auth.Concrete;
using Ninject.Modules;

namespace GameStore.IoC
{
    public class AuthModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAuthenticationService>().To<AuthenticationService>();
        }
    }
}
