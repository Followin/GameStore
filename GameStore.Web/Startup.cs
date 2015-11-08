using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using GameStore.BLL.CQRS;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(GameStore.Web.Startup))]

namespace GameStore.Web
{
    public class HubActivator : IHubActivator
    {
        public IHub Create(HubDescriptor descriptor)
        {
            return (IHub)DependencyResolver.Current
                    .GetService(descriptor.HubType);
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var activator = new HubActivator();

            GlobalHost.DependencyResolver.Register(
                typeof(IHubActivator),
                () => activator);
            app.MapSignalR();
        }
    }
}
