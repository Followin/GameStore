using System;
using System.Linq;
using System.Linq.Expressions;
using GameStore.BLL.CommandHandlers;
using GameStore.BLL.Commands;
using GameStore.BLL.Queries;
using GameStore.BLL.Queries.Comment;
using GameStore.BLL.QueryHandlers;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;

namespace GameStore.Tests.BLLTests
{
    [TestClass]
    public class CommentTests
    {
        private Mock<IRepository<Game, Int32>> gameRepositoryMock;
        private Mock<IRepository<Comment, Int32>> commentRepositoryMock;
        private Mock<IGameStoreUnitOfWork> unitOfWorkMock;
        private Comment[] comments;
        private CreateCommentCommand createCommentCommand;
        private CommentCommandHandler commandHandler;
        private CommentQueryHandler queryHandler;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            AutoMapperConfiguration.Configure();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var dota = new Game
            {
                Id = 1,
                Name = "Dota 2",
                Description = "Just try it",
                Key = "dota-2"
            };
            var games = new[] { dota };
            var innerComment = new Comment
            {
                Id = 2,
                Name = "Second author",
                Body = "Inner comment"
            };
            var comment = new Comment
            {
                Id = 1,
                Name = "First author",
                Body = "Outer comment",
                Game = dota,
                GameId = dota.Id,
                ChildComments = new[] { innerComment }
            };

            createCommentCommand = new CreateCommentCommand
            {
                Name = "New name",
                Body = "New body",
                GameId = 1
            };

            innerComment.ParentComment = comment;
            innerComment.ParentCommentId = comment.Id;
            comments = new[] { comment, innerComment };

            gameRepositoryMock = new Mock<IRepository<Game, int>>();
            gameRepositoryMock.Setup(x => x.GetSingle(It.IsAny<Expression<Func<Game, Boolean>>>())).Returns(
                (Expression<Func<Game, Boolean>> predicate) => games.FirstOrDefault(predicate.Compile()));
            gameRepositoryMock.Setup(x => x.Get(It.Is<Int32>(_ => _ == 1))).Returns(dota);

            commentRepositoryMock = new Mock<IRepository<Comment, int>>();
            commentRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Comment, Boolean>>>())).Returns(
                (Expression<Func<Comment, Boolean>> predicate) => comments.Where(predicate.Compile()));
            commentRepositoryMock.Setup(x => x.Get(It.IsAny<Int32>())).Returns(
                (Int32 i) => comments.FirstOrDefault(c => c.Id == i));

            unitOfWorkMock = new Mock<IGameStoreUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Games).Returns(gameRepositoryMock.Object);
            unitOfWorkMock.Setup(x => x.Comments).Returns(commentRepositoryMock.Object);

            ILogger logger = new Mock<ILogger>().Object;

            queryHandler = new CommentQueryHandler(unitOfWorkMock.Object, logger);
            commandHandler = new CommentCommandHandler(unitOfWorkMock.Object, logger);
        }

        [TestMethod]
        public void Create_Comment_Name_Argument_Is_Null()
        {
            // Arrange
            createCommentCommand.Name = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                commandHandler.Execute(createCommentCommand));

            // Assert
            unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            Assert.AreEqual("Name", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_Name_Argument_Is_Empty()
        {
            // Arrange
            createCommentCommand.Name = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(createCommentCommand));

            // Assert
            unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            Assert.AreEqual("Name", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_Body_Argument_Is_Null()
        {
            // Arrange
            createCommentCommand.Body = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                commandHandler.Execute(createCommentCommand));

            // Assert
            unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            Assert.AreEqual("Body", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_Body_Argument_Is_Empty()
        {
            // Arrange
            createCommentCommand.Body = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                commandHandler.Execute(createCommentCommand));

            // Assert
            unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            Assert.AreEqual("Body", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_GameId_And_ParentCommendId_Are_Null()
        {
            // Arrange
            createCommentCommand.GameId = null;
            createCommentCommand.ParentCommentId = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                commandHandler.Execute(createCommentCommand));

            // Assert
            unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            Assert.AreEqual("GameId, ParentCommentId", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_GameId_Argument_Is_Zero()
        {
            // Arrange
            createCommentCommand.GameId = 0;
            
            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                commandHandler.Execute(createCommentCommand));

            // Assert
            unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("GameId", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_GameId_Argument_Is_Negative()
        {
            // Arrange
            createCommentCommand.GameId = -1;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                commandHandler.Execute(createCommentCommand));

            // Assert
            unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("GameId", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_ParentCommendId_Argument_Is_Zero()
        {
            // Arrange
            createCommentCommand.GameId = null;
            createCommentCommand.ParentCommentId = 0;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                commandHandler.Execute(createCommentCommand));

            // Assert
            unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("ParentCommentId", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_ParentCommentId_Argument_Is_Negative()
        {
            // Arrange
            createCommentCommand.GameId = null;
            createCommentCommand.ParentCommentId = -1;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                commandHandler.Execute(createCommentCommand));

            // Assert
            unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("ParentCommentId", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_GameId_Doesnt_Match_Existing_Game()
        {
            // Arrange
            createCommentCommand.GameId = 5;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                commandHandler.Execute(createCommentCommand));

            // Assert
            gameRepositoryMock.Verify(x => x.Get(It.IsAny<Int32>()), Times.Once);
            Assert.AreEqual("GameId", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_ParentCommentId_Doesnt_Match_Existing_Comment()
        {
            // Arrange
            createCommentCommand.GameId = null;
            createCommentCommand.ParentCommentId = 5;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                commandHandler.Execute(createCommentCommand));

            // Assert
            commentRepositoryMock.Verify(x => x.Get(It.IsAny<Int32>()), Times.Once);
            Assert.AreEqual("ParentCommentId", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_Using_GameId()
        {
            // Arrange
            // Act
            commandHandler.Execute(createCommentCommand);
            
            // Assert
            commentRepositoryMock.Verify(x => x.Add(It.IsAny<Comment>()), Times.Once);
            unitOfWorkMock.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void Create_Comment_Using_ParentCommentId()
        {
            // Arrange
            createCommentCommand.GameId = null;
            createCommentCommand.ParentCommentId = 1;

            // Act
            commandHandler.Execute(createCommentCommand);

            // Assert
            commentRepositoryMock.Verify(x => x.Add(It.IsAny<Comment>()), Times.Once);
            unitOfWorkMock.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void GetCommentsByGameKey_Key_Argument_Is_Null()
        {
            // Arrange
            var getCommentsByGameKey = new GetCommentsByGameKeyQuery { Key = null };

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                queryHandler.Retrieve(getCommentsByGameKey));

            // Assert
            unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void GetCommentsByGameKey_Key_Argument_Is_Empty()
        {
            // Arrange
            var getCommentsByGameKey = new GetCommentsByGameKeyQuery { Key = String.Empty };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                queryHandler.Retrieve(getCommentsByGameKey));

            // Assert
            unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void GetCommentsByGameKey_Key_Argument_Doesnt_Match_Existing_Game()
        {
            // Arrange
            var getCommentsByGameKey = new GetCommentsByGameKeyQuery { Key = "notExisingGame" };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                queryHandler.Retrieve(getCommentsByGameKey));

            // Assert
            gameRepositoryMock.Verify(x => x.GetSingle(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void GetCommentsByGameKey_Right_Data()
        {
            // Arrange
            var getCommentsByGameKey = new GetCommentsByGameKeyQuery { Key = "dota-2" };

            // Act
            var result = queryHandler.Retrieve(getCommentsByGameKey);

            // Assert
            Assert.AreEqual(1, result.Count());
        }
    }
}
