using System.Web.Routing;
using GameStore.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcRouteTester;

namespace GameStore.Tests
{
    [TestClass]
    public class RoutesTests
    {
        private static RouteCollection routes;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
        }

        [TestMethod]
        public void Default()
        {
            RouteAssert.HasRoute(
                routes,
                "/",
                new { controller = "Game", action = "Index" });
        }

        [TestMethod]
        public void CreateComment()
        {
            RouteAssert.HasRoute(
                routes,
                "/game/somegamekey/newcomment",
                new { controller = "Game", action = "NewComment", gamekey = "somegamekey" });
        }

        [TestMethod]
        public void Game()
        {
            RouteAssert.HasRoute(
                routes,
                "/Game/somegamekey/someaction",
                new { controller = "Game", action = "someaction", gamekey = "somegamekey" });
        }

        

        
    }
}
