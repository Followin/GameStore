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
                name: "lang:Game",
                url: "{lang}/{controller}/{gamekey}/{action}",
                defaults: new { action = "Details" },
                constraints: new { controller = @"[Gg]ame", lang = @"ru|en" });

            routes.MapRoute(
                name: "lang:Games",
                url: "{lang}/Games/{action}",
                defaults: new { controller = "game", action = "index", lang = @"ru|en" },
                constraints: new { lang = @"ru||en" });

            routes.MapRoute(
                name: "lang:Publisher",
                url: "lang:{lang}/{controller}/{companyname}/{action}",
                defaults: new { action = "Details" },
                constraints: new { controller = @"[Pp]ublisher", lang = @"ru|en" });

            routes.MapRoute(
                name: "lang:Order",
                url: "{lang}/order/checkout/{paymentMethodKey}",
                defaults: new { controller = "order", action = "checkout" },
                constraints: new { lang = @"ru|en" });

            routes.MapRoute(
                name: "lang:Default",
                url: "{lang}/{controller}/{action}/{id}",
                defaults: new { controller = "Game", action = "Index", id = UrlParameter.Optional },
                constraints: new { lang = @"ru|en" });

            //no-lang routes

            routes.MapRoute(
                name: "Game",
                url: "{controller}/{gamekey}/{action}",
                defaults: new { action = "Details", lang="en" },
                constraints: new { controller = "[Gg]ame"});

            routes.MapRoute(
                name: "Publisher",
                url: "{controller}/{companyname}/{action}",
                defaults: new { action = "Details", lang = "en" },
                constraints: new { controller = @"[Pp]ublisher" });

            routes.MapRoute(
                name: "Games",
                url: "games/{action}",
                defaults: new { controller = "game", action = "index", lang = "en" });

            routes.MapRoute(
                name: "Order",
                url: "order/checkout/{paymentMethodKey}",
                defaults: new { controller = "order", action = "checkout", lang = "en" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Game", action = "Index", id = UrlParameter.Optional, lang = "en" });
        }
    }
}