using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Services.Protocols;

namespace GameStore.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Game",
                url: "{controller}/{gamekey}/{action}",
                defaults: new { action = "Details" },
                constraints: new { controller = "Game" });

            routes.MapRoute(
                name: "Games",
                url: "Games/{action}",
                defaults: new {controller = "game", action = "index"});

            routes.MapRoute(
                name: "Publisher",
                url: "{controller}/{companyname}/{action}",
                defaults: new { action = "Details" },
                constraints: new { controller = "Publisher" });

            routes.MapRoute(
                name: "Order",
                url: "order/checkout/{paymentMethodKey}",
                defaults: new { controller = "order", action = "checkout" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Game", action = "Index", id = UrlParameter.Optional });
        }
    }
}