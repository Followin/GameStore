using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries;
using GameStore.BLL.Queries.Game;
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
        private GameController gameController;
        private GamesController gamesController;
        private Mock<IQueryDispatcher> queryDispatcherMock;
        private Mock<ICommandDispatcher> commandDispatcherMock;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            AutoMapperConfiguration.Configure();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            queryDispatcherMock = new Mock<IQueryDispatcher>();
            commandDispatcherMock = new Mock<ICommandDispatcher>();
            var loggerMock = new Mock<ILogger>();
            gameController = new GameController(
                commandDispatcherMock.Object,
                queryDispatcherMock.Object,
                loggerMock.Object);
            gamesController = new GamesController(
                commandDispatcherMock.Object,
                queryDispatcherMock.Object,
                loggerMock.Object);
        }

        [TestMethod]
        public void Details_Returns_Model()
        {
            // Arrange
            queryDispatcherMock.Setup(x => x.Dispatch<GetGameByKeyQuery, GameQueryResult>(It.IsAny<GetGameByKeyQuery>()))
                           .Returns(new GameQueryResult { Id = 1, Name = "SomeName" });

            // Act
            var result = gameController.Details("someKey") as JsonResult;

            // Assert
            Assert.AreEqual("SomeName", ((DisplayGameViewModel)result.Data).Name);
        }



        

        [TestMethod]
        public void CreateGame_Redirects_After()
        {
            // Arrange

            // Act
            var result = (RedirectToRouteResult)gamesController.Create(new CreateGameViewModel
            {
                CreateModel = new CreateGameModel
                {
                    Description = "New game description",
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
        public void EditGame_Redirect_After()
        {
            var result = (RedirectToRouteResult)gamesController.Edit(new EditGameViewModel
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
            var result = (RedirectToRouteResult)gamesController.Remove("game-key");

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }
}
