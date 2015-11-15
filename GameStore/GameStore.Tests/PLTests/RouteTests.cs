using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using GameStore.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests.PLTests
{
    [TestClass]
    public class RouteTests
    {
        private static RouteCollection routes;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
        }

        private static void AssertRoute(RouteCollection routeCollection, string url, object expectations)
        {
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath)
                .Returns(url);

            RouteData routeData = routeCollection.GetRouteData(httpContextMock.Object);
            Assert.IsNotNull(routeData);

            foreach (var kvp in new RouteValueDictionary(expectations))
            {
                Assert.IsTrue(string.Equals(kvp.Value.ToString(),
                                          routeData.Values[kvp.Key].ToString(),
                                          StringComparison.OrdinalIgnoreCase)
                            , string.Format("Expected '{0}', not '{1}' for '{2}'.",
                                            kvp.Value, routeData.Values[kvp.Key], kvp.Key));
            }
        }


        [TestMethod]
        public void Games_Game_Chained()
        {
            AssertRoute(routes, "~/games/someaction",
                new {controller = "game", action = "someaction"});
        }

        [TestMethod]
        public void GameKey_Route_Section()
        {
            AssertRoute(routes, "~/game/mygamekey/someaction",
                new {controller = "game", gamekey="mygamekey", action="someaction"});
        }

        [TestMethod]
        public void Game_Default_Action()
        {
            AssertRoute(routes, "~/game/mygamekey",
                new {controller="game", action="details"});
        }

        [TestMethod]
        public void Publisher_Default_Action()
        {
            AssertRoute(routes, "~/publisher/mygamekey",
                new { controller = "publisher", action = "details" });
        }

        [TestMethod]
        public void Default_Route()
        {
            AssertRoute(routes, "~/account/login",
                new {controller="account", action="login"});
        }
    }
}
