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
    }
}
