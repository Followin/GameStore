using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.DAL.Abstract;
using GameStore.DAL.EF;
using GameStore.DAL.Repositories;
using GameStore.Domain.Abstract;
using Ninject.Modules;

namespace GameStore.IoC
{
    public class DALModule : NinjectModule
    {
        private String _connectionString;

        public DALModule(String connectionString)
        {
            this._connectionString = connectionString;
        }

        public override void Load()
        {
            Bind<IContext>().To<EFContext>().WithConstructorArgument("connectionString", _connectionString);
            Bind<IGameStoreUnitOfWork>().To<GameStoreUnitOfWork>();
        }
    }
}
