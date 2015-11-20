using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries;
using GameStore.BLL.Queries.Comment;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.Queries.Order;
using GameStore.BLL.QueryHandlers.Comment;
using GameStore.BLL.QueryResults;
using GameStore.BLL.QueryResults.Comment;
using GameStore.BLL.QueryResults.Game;
using GameStore.BLL.QueryResults.Order;
using GameStore.Static;
using GameStore.Tests.Utils;
using GameStore.Web.Controllers;
using GameStore.Web.Models;
using GameStore.Web.Models.Comment;
using GameStore.Web.Models.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;

namespace GameStore.Tests.PLTests
{
    [TestClass]
    public class ControllerTests
    {
        private GameController _gameController;
        private Mock<IQueryDispatcher> _queryDispatcherMock;
        private Mock<ICommandDispatcher> _commandDispatcherMock;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            AutoMapperConfiguration.Configure();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _queryDispatcherMock = new Mock<IQueryDispatcher>();
            _commandDispatcherMock = new Mock<ICommandDispatcher>();
            var loggerMock = new Mock<ILogger>();
            _gameController = new GameController(
                _commandDispatcherMock.Object,
                _queryDispatcherMock.Object,
                loggerMock.Object);
        }



        [TestMethod]
        public void CreateGame_Redirects_After()
        {
            // Arrange

            // Act
            var result = (RedirectToRouteResult)_gameController.Create(new CreateGameViewModel
            {
                CreateModel = new CreateGameModel
                {
                    DescriptionEn = "New game description",
                    GenreIds = new[] { 1, 2 },
                    Key = "new-game",
                    Name = "New game",
                    PlatformTypeIds = new[] { 1, 3 }
                }
            });

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }


        [TestMethod]
        public void Delete_Redirect_After()
        {
            var result = (RedirectToRouteResult)_gameController.Remove("game-key");

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Details_View_Not_Null()
        {
            // Arrange
            _queryDispatcherMock.Setup(
                x => x.Dispatch<GetGameByKeyQuery, GameQueryResult>(It.IsAny<GetGameByKeyQuery>()))
                                .Returns(new GameQueryResult { Name = "sss" });

            // Act
            var result = (ViewResult)_gameController.Details("First");


            // Assert
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void Comments_View_Not_Null()
        {
            // Arrange
            _queryDispatcherMock.Setup(
                x => x.Dispatch<GetCommentsForGameQuery, CommentsQueryResult>(It.IsAny<GetCommentsForGameQuery>()))
                                .Returns(new CommentsQueryResult(new[] { new CommentDTO { Name = "First" } }));

            // Act
            var result = (ViewResult)_gameController.Comments("First");


            // Assert
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void Buy_Redirects_After()
        {
            // Arrange
            _queryDispatcherMock.Setup(
                x => x.Dispatch<GetGameByKeyQuery, GameQueryResult>(It.IsAny<GetGameByKeyQuery>()))
                                .Returns(new GameQueryResult { Name = "sss", EntryState = EntryState.Active });
            var fakeHttpContext = new Mock<HttpContextBase>();
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.SerialNumber, "1") }.ToList()));

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);
            _gameController.ControllerContext = controllerContext.Object;

            _queryDispatcherMock.Setup(x => x.Dispatch<GetCurrentOrderQuery, OrderQueryResult>(It.IsAny<GetCurrentOrderQuery>()))
                                .Returns(new OrderQueryResult {Id = 1, UserId = 1});

            // Act
            var result = (RedirectToRouteResult)_gameController.Buy("First");


            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Game", result.RouteValues["controller"]);
        }
    }
}
