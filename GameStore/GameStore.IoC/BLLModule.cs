using GameStore.BLL.Abstract;
using GameStore.BLL.Concrete;
using GameStore.BLL.CQRS;
using GameStore.BLL.Observer;
using Ninject;
using Ninject.Modules;
using Ninject.Extensions.Conventions;

namespace GameStore.IoC
{
    public class BLLModule : NinjectModule
    {
        
        public override void Load()
        {
            Kernel.Bind(_ => _.FromAssembliesMatching("GameStore.BLL.dll")
                             .SelectAllClasses().InheritedFrom(typeof(IQueryHandler<,>))
                             .BindAllInterfaces());
            Kernel.Bind(_ => _.FromAssembliesMatching("GameStore.BLL.dll")
                              .SelectAllClasses().InheritedFrom(typeof(ICommandHandler<>))
                              .BindAllInterfaces());
            Kernel.Bind(_ => _.FromAssembliesMatching("GameStore.BLL.dll")
                              .SelectAllClasses()
                              .BindDefaultInterface());
            Kernel.Bind<OrderNotificationSiren>().To<OrderNotificationSiren>().InSingletonScope();
        }
    }
}
