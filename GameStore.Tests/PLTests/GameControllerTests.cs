using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries;
using GameStore.BLL.QueryResults;
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
    public class GameControllerTests
    {
        private GameController _gameController;
        private GamesController _gamesController;
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
            _gamesController = new GamesController(
                _commandDispatcherMock.Object,
                _queryDispatcherMock.Object,
                loggerMock.Object);
        }

        [TestMethod]
        public void Details_Returns_Model()
        {
            // Arrange
            _queryDispatcherMock.Setup(x => x.Dispatch<GetGameByKeyQuery, GameQueryResult>(It.IsAny<GetGameByKeyQuery>()))
                           .Returns(new GameQueryResult { Id = 1, Name = "SomeName" });

            // Act
            var result = _gameController.Details("someKey") as JsonResult;

            // Assert
            Assert.AreEqual("SomeName", ((DisplayGameViewModel)result.Data).Name);
        }

        [TestMethod]
        public void CreateComment_Redirects_After()
        {
            // Arrange
            
            // Act
            var result =
                _gameController.CreateComment(new CreateCommentViewModel { Body = "body", GameId = 1, Name = "name" }) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Games", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void CreateComment_With_2_Args_Redirects_After()
        {
            // Arrange

            // Act
            var result =
                _gameController.CreateComment("somekey", new CreateCommentViewModel { Body = "body", GameId = 1, Name = "name" }) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Games", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void CreateGame_Redirects_After()
        {
            // Arrange

            // Act
            var result = (RedirectToRouteResult)_gamesController.Create(new CreateGameViewModel
            {
                Description = "New game description",
                GenreIds = new[] { 1, 2 },
                Key = "new-game",
                Name = "New game",
                PlatformTypeIds = new[] { 1, 3 }
            });

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void EditGame_Redirect_After()
        {
            var result = (RedirectToRouteResult)_gamesController.Edit(new EditGameViewModel
            {
                Id = 1,
                Description = "New game description",
                GenreIds = new[] { 1, 2 },
                Key = "new-game",
                Name = "New game",
                PlatformTypeIds = new[] { 1, 3 }
            });

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Delete_Redirect_After()
        {
            var result = (RedirectToRouteResult)_gamesController.Remove("game-key");

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }
}
