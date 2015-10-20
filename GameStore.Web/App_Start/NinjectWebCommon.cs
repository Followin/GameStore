using System.Diagnostics;
using NLog;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(GameStore.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(GameStore.Web.App_Start.NinjectWebCommon), "Stop")]

namespace GameStore.Web.App_Start
{
    using System;
    using System.Web;
    using BLL.Utils;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Modules;
    using Ninject.Web.Common;
    using Ninject.Extensions.Conventions;
    using BLL.CQRS;
    using GameStore.BLL.Commands;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var modules = new INinjectModule[] { new BLLNinjectModule("DefaultConnection") };
            var kernel = new StandardKernel(modules);
            
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind(_ => _.FromAssembliesMatching("GameStore.BLL.dll")
                              .SelectAllClasses().InheritedFrom(typeof(IQueryHandler<,>))
                              .BindAllInterfaces());
            kernel.Bind(_ => _.FromAssembliesMatching("GameStore.BLL.dll")
                              .SelectAllClasses().InheritedFrom(typeof(ICommandHandler<>))
                              .BindAllInterfaces());
            kernel.Bind(_ => _.FromAssembliesMatching("GameStore.BLL.dll")
                              .SelectAllClasses()
                              .BindDefaultInterface());
            kernel.Bind<ILogger>().ToMethod(context =>
            {
                return LogManager.GetLogger(context.Request.ParentContext.Request.Service.FullName);
            });
        }        
    }
}