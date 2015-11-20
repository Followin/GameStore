using System;
using GameStore.DAL.Abstract;
using GameStore.DAL.EF;
using GameStore.DAL.Northwind.Repositories;
using GameStore.DAL.Repositories;
using Ninject.Modules;

namespace GameStore.IoC
{
    public class DALModule : NinjectModule
    {
        private string _connectionString;

        public DALModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        public override void Load()
        {
            Bind<IEFContext>().To<EFContext>().WithConstructorArgument("connectionString", _connectionString);
            Bind<IGameStoreUnitOfWork>().To<GameStoreUnitOfWork>();
            Bind<INorthwindUnitOfWork>().To<NorthwindUnitOfWork>();

        }
    }
}
