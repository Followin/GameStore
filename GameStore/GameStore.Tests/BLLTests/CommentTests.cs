using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using GameStore.BLL.CommandHandlers;
using GameStore.BLL.CommandHandlers.Comment;
using GameStore.BLL.Commands;
using GameStore.BLL.Commands.Comment;
using GameStore.BLL.Queries;
using GameStore.BLL.Queries.Comment;
using GameStore.BLL.QueryHandlers;
using GameStore.BLL.QueryHandlers.Comment;
using GameStore.BLL.Utils;
using GameStore.Domain.Abstract;
using GameStore.Domain.Abstract.Repositories;
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
        private Mock<IGameRepository> _gameRepositoryMock;
        private Mock<ICommentRepository> _commentRepositoryMock;
        private Mock<IGameStoreUnitOfWork> _unitOfWorkMock;
        private Comment[] _comments;
        private CreateCommentCommand _createCommentCommand;
        private CreateCommentHandler _commandHandler;
        private GetCommentsByGameKeyQueryHandler _queryHandler;

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

            _createCommentCommand = new CreateCommentCommand
            {
                Name = "New name",
                Body = "New body",
                GameId = 1
            };

            innerComment.ParentComment = comment;
            innerComment.ParentCommentId = comment.Id;
            _comments = new[] { comment, innerComment };

            _gameRepositoryMock = new Mock<IGameRepository>();
            _gameRepositoryMock.Setup(x => x.GetSingle(It.IsAny<Expression<Func<Game, Boolean>>>())).Returns(
                (Expression<Func<Game, Boolean>> predicate) => games.FirstOrDefault(predicate.Compile()));
            _gameRepositoryMock.Setup(x => x.Get(It.Is<Int32>(i => i == 1))).Returns(dota);

            _commentRepositoryMock = new Mock<ICommentRepository>();
            _commentRepositoryMock.Setup(x => x.Get(
                It.IsAny<Expression<Func<Comment, Boolean>>>())).Returns(
                (Expression<Func<Comment, Boolean>> predicate) => _comments.Where(predicate.Compile()));
            _commentRepositoryMock.Setup(x => x.Get(It.IsAny<Int32>())).Returns(
                (Int32 i) => _comments.FirstOrDefault(c => c.Id == i));

            _unitOfWorkMock = new Mock<IGameStoreUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.Games).Returns(_gameRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.Comments).Returns(_commentRepositoryMock.Object);

            ILogger logger = new Mock<ILogger>().Object;

            _queryHandler = new GetCommentsByGameKeyQueryHandler(_unitOfWorkMock.Object, logger);
            _commandHandler = new CreateCommentHandler(_unitOfWorkMock.Object, logger);
        }

        [TestMethod]
        public void Create_Comment_Name_Argument_Is_Null()
        {
            // Arrange
            _createCommentCommand.Name = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _commandHandler.Execute(_createCommentCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            Assert.AreEqual("Name", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_Name_Argument_Is_Empty()
        {
            // Arrange
            _createCommentCommand.Name = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_createCommentCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            Assert.AreEqual("Name", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_Body_Argument_Is_Null()
        {
            // Arrange
            _createCommentCommand.Body = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _commandHandler.Execute(_createCommentCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            Assert.AreEqual("Body", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_Body_Argument_Is_Empty()
        {
            // Arrange
            _createCommentCommand.Body = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_createCommentCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            Assert.AreEqual("Body", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_GameId_And_ParentCommendId_Are_Null()
        {
            // Arrange
            _createCommentCommand.GameId = null;
            _createCommentCommand.ParentCommentId = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _commandHandler.Execute(_createCommentCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            Assert.AreEqual("GameId, ParentCommentId", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_GameId_Argument_Is_Zero()
        {
            // Arrange
            _createCommentCommand.GameId = 0;
            
            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _commandHandler.Execute(_createCommentCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("GameId", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_GameId_Argument_Is_Negative()
        {
            // Arrange
            _createCommentCommand.GameId = -1;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _commandHandler.Execute(_createCommentCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("GameId", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_ParentCommendId_Argument_Is_Zero()
        {
            // Arrange
            _createCommentCommand.GameId = null;
            _createCommentCommand.ParentCommentId = 0;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _commandHandler.Execute(_createCommentCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("ParentCommentId", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_ParentCommentId_Argument_Is_Negative()
        {
            // Arrange
            _createCommentCommand.GameId = null;
            _createCommentCommand.ParentCommentId = -1;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _commandHandler.Execute(_createCommentCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("ParentCommentId", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_GameId_Doesnt_Match_Existing_Game()
        {
            // Arrange
            _createCommentCommand.GameId = 5;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _commandHandler.Execute(_createCommentCommand));

            // Assert
            _gameRepositoryMock.Verify(x => x.Get(It.IsAny<Int32>()), Times.Once);
            Assert.AreEqual("GameId", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_ParentCommentId_Doesnt_Match_Existing_Comment()
        {
            // Arrange
            _createCommentCommand.GameId = null;
            _createCommentCommand.ParentCommentId = 5;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _commandHandler.Execute(_createCommentCommand));

            // Assert
            _commentRepositoryMock.Verify(x => x.Get(It.IsAny<Int32>()), Times.Once);
            Assert.AreEqual("ParentCommentId", result.ParamName);
        }

        [TestMethod]
        public void Create_Comment_Using_GameId()
        {
            // Arrange
            // Act
            _commandHandler.Execute(_createCommentCommand);
            
            // Assert
            _commentRepositoryMock.Verify(x => x.Add(It.IsAny<Comment>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void Create_Comment_Using_ParentCommentId()
        {
            // Arrange
            _createCommentCommand.GameId = null;
            _createCommentCommand.ParentCommentId = 1;

            // Act
            _commandHandler.Execute(_createCommentCommand);

            // Assert
            _commentRepositoryMock.Verify(x => x.Add(It.IsAny<Comment>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void GetCommentsByGameKey_Key_Argument_Is_Null()
        {
            // Arrange
            var getCommentsByGameKey = new GetCommentsByGameKeyQuery { Key = null };

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _queryHandler.Retrieve(getCommentsByGameKey));

            // Assert
            _unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void GetCommentsByGameKey_Key_Argument_Is_Empty()
        {
            // Arrange
            var getCommentsByGameKey = new GetCommentsByGameKeyQuery { Key = String.Empty };

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _queryHandler.Retrieve(getCommentsByGameKey));

            // Assert
            _unitOfWorkMock.Verify(x => x.Comments, Times.Never);
            _unitOfWorkMock.Verify(x => x.Games, Times.Never);
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void GetCommentsByGameKey_Key_Argument_Doesnt_Match_Existing_Game()
        {
            // Arrange
            var getCommentsByGameKey = new GetCommentsByGameKeyQuery { Key = "notExisingGame" };

            // Act
            var result = ExceptionAssert.Throws<EntityNotFoundException>(() =>
                _queryHandler.Retrieve(getCommentsByGameKey));

            // Assert
            _gameRepositoryMock.Verify(x => x.GetSingle(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
            Assert.AreEqual("Key", result.ParamName);
        }

        [TestMethod]
        public void GetCommentsByGameKey_Right_Data()
        {
            // Arrange
            var getCommentsByGameKey = new GetCommentsByGameKeyQuery { Key = "dota-2" };

            // Act
            var result = _queryHandler.Retrieve(getCommentsByGameKey);

            // Assert
            Assert.AreEqual(1, result.Count());
        }
    }
}
