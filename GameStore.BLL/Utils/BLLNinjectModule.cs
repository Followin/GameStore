using System;
using GameStore.DAL.Abstract;
using GameStore.DAL.EF;
using GameStore.DAL.Repositories;
using GameStore.Domain.Abstract;
using Ninject.Modules;

namespace GameStore.BLL.Utils
{
    public class BLLNinjectModule : NinjectModule
    {
        private String connectionString;

        public BLLNinjectModule(String connectionString)
        {
            this.connectionString = connectionString;
        }

        public override void Load()
        {
            Bind<IContext>().To<EFContext>().WithConstructorArgument("connectionString", connectionString);
            Bind<IGameStoreUnitOfWork>().To<GameStoreUnitOfWork>();
        }
    }
}
