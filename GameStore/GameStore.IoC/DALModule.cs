using System;
using GameStore.DAL.Abstract;
using GameStore.DAL.EF;
using GameStore.DAL.Northwind.Repositories;
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
            _connectionString = connectionString;
        }

        public override void Load()
        {
            Bind<EFContext>().To<EFContext>().WithConstructorArgument("connectionString", _connectionString);
            Bind<IGameStoreUnitOfWork>().To<GameStoreUnitOfWork>();
            Bind<INorthwindUnitOfWork>().To<NorthwindUnitOfWork>();

        }
    }
}
