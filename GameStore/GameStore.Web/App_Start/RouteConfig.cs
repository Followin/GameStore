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
                url: "{lang}/{controller}/{gamekey}/{action}",
                defaults: new { action = "Details" },
                constraints: new { controller = "Game", lang = @"ru|en" });

            routes.MapRoute(
                name: "Games",
                url: "{lang}/Games/{action}",
                defaults: new { controller = "game", action = "index", lang = @"ru|en" },
                constraints: new { lang = @"ru||en" });

            routes.MapRoute(
                name: "Publisher",
                url: "{lang}/{controller}/{companyname}/{action}",
                defaults: new { action = "Details" },
                constraints: new { controller = "Publisher", lang = @"ru|en" });

            routes.MapRoute(
                name: "Order",
                url: "{lang}/order/checkout/{paymentMethodKey}",
                defaults: new { controller = "order", action = "checkout" },
                constraints: new { lang = @"ru|en" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Game", action = "Index", id = UrlParameter.Optional, lang = "en" });
        }
    }
}