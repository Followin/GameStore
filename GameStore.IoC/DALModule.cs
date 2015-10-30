using System;
using GameStore.DAL.Abstract;
using GameStore.DAL.EF;
using GameStore.DAL.Repositories;
using GameStore.Domain.Abstract;
using Ninject.Extensions.Conventions;
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
            Kernel.Bind(_ => _.FromAssembliesMatching("GameStore.DAL.dll")
                              .SelectAllClasses()
                              .Where(x => x.Name != "EFContext")
                              .BindDefaultInterfaces());
            Bind<IContext>().To<EFContext>().InThreadScope().WithConstructorArgument("connectionString", _connectionString);
            
        }
    }
}
