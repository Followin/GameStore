using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using GameStore.BLL.Utils;
using GameStore.Maps;
using GameStore.Web.ModelBinders;
using GameStore.Web.Models.Order;
using GameStore.Web.Utils;

namespace GameStore.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(OrderViewModel), new CurrentOrderModelBinder());
            ValueProviderFactories.Factories.Add(new SessionIdValueProviderFactory());

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new BLLProfile());
                cfg.AddProfile(new WebMapperProfile());
            });
            //Mapper.AssertConfigurationIsValid();
        }
    }
}