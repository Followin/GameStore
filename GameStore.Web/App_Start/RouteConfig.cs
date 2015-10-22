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
                name: "Create",
                url: "{controller}/new",
                defaults: new { action = "Create" });

            routes.MapRoute(
                name: "Edit",
                url: "{controller}/update",
                defaults: new { action = "Edit" });

            routes.MapRoute(
                name: "Game",
                url: "{controller}/{gamekey}/{action}",
                defaults: new { action = "Details" },
                constraints: new { controller = "Game" });

            routes.MapRoute(
                name: "Publisher",
                url: "{controller}/{companyname}/{action}",
                defaults: new { action = "Details" },
                constraints: new { controller = "Publisher" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Games", action = "Index", id = UrlParameter.Optional });
        }
    }
}