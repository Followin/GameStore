using System.Net.Http.Formatting;
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
                defaults: new { id = RouteParameter.Optional, controller = "comments" },
                constraints: new { gameid = @"\d+" });

            config.Routes.MapHttpRoute(
                name: "LangGameGenres",
                routeTemplate: "api/{lang}/games/{gameid}/genres",
                defaults: new { controller = "gamegenres" },
                constraints: new { lang = @"en||ru", gameid = @"\d+" });
            config.Routes.MapHttpRoute(
                name: "GameGenres",
                routeTemplate: "api/games/{gameid}/genres",
                defaults: new { controller = "gamegenres" },
                constraints: new { gameid = @"\d+" });

            config.Routes.MapHttpRoute(
                name: "LangPublisherGames",
                routeTemplate: "api/{lang}/publishers/{publisherid}/games",
                defaults: new { controller = "publishergames" },
                constraints: new { lang = @"en||ru", publisherid = @"\d+" });
            config.Routes.MapHttpRoute(
                name: "PublisherGames",
                routeTemplate: "api/publishers/{publisherid}/games",
                defaults: new { controller = "publishergames" },
                constraints: new { publisherid = @"\d+" });

            config.Routes.MapHttpRoute(
                name: "LangGenreGames",
                routeTemplate: "api/{lang}/genres/{genreid}/games",
                defaults: new { controller = "genregames" },
                constraints: new { lang = @"en||ru", genreid = @"\d+" });
            config.Routes.MapHttpRoute(
                name: "GenreGames",
                routeTemplate: "api/genres/{genreid}/games",
                defaults: new { controller = "genregames" },
                constraints: new { genreid = @"\d+" });


            GlobalConfiguration.Configuration.Filters.Add(new LocalizeApiAttribute());

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("type", "json", new MediaTypeHeaderValue("application/json")));

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("type", "xml", new MediaTypeHeaderValue("application/xml")));

            GlobalConfiguration.Configuration.Services.RemoveAll(
                typeof(System.Web.Http.Validation.ModelValidatorProvider),
                v => v is InvalidModelValidatorProvider);

        }
    }
}
