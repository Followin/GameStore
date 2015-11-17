using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Validation.Providers;
using GameStore.Web.Filters;

namespace GameStore.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                   name: "LangApi",
                   routeTemplate: "api/{lang}/{controller}/{id}",
                   defaults: new { id = RouteParameter.Optional },
                   constraints: new { lang = @"en||ru" });

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: "LangComments",
                routeTemplate: "api/{lang}/games/{gameid}/comments/{id}",
                defaults: new { id = RouteParameter.Optional, controller = "comments" },
                constraints: new { lang = @"en||ru", gameid = @"\d+" });

            config.Routes.MapHttpRoute(
                name: "Comments",
                routeTemplate: "api/games/{gameid}/comments/{id}",
                defaults: new {id = RouteParameter.Optional, controller = "comments"},
                constraints: new {gameid = @"\d+"});

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            GlobalConfiguration.Configuration.Filters.Add(new LocalizeApiAttribute());

            GlobalConfiguration.Configuration.Services.RemoveAll(
                typeof (System.Web.Http.Validation.ModelValidatorProvider),
                v => v is InvalidModelValidatorProvider);

        }
    }
}
