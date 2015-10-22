﻿using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using GameStore.BLL.Utils;
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
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(OrderViewModel), new CurrentOrderModelBinder());

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new BLLMapperProfile());
                cfg.AddProfile(new WebMapperProfile());
            });
        }
    }
}