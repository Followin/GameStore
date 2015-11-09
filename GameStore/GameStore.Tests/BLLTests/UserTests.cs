using System;
using System.Linq;
using System.Linq.Expressions;
using GameStore.BLL.CommandHandlers;
using GameStore.BLL.CommandHandlers.User;
using GameStore.BLL.Commands.User;
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
    public class UserTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IGameStoreUnitOfWork> _unitOfWorkMock;
        private CreateUserCommand _rightCreateCommandSample;
        private CreateUserCommandHandler _commandHandler;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            AutoMapperConfiguration.Configure();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var users = new[]
            {
                new User
                {
                    Id = 1,
                    SessionId = "someSessionId"
                }
            };
            _userRepositoryMock = new Mock<IUserRepository>();
            _userRepositoryMock.Setup(x => x.GetSingle(It.IsAny<Expression<Func<User, Boolean>>>()))
                               .Returns(
                                   (Expression<Func<User, Boolean>> predicate) =>
                                   users.FirstOrDefault(predicate.Compile()));

            _unitOfWorkMock = new Mock<IGameStoreUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.Users).Returns(_userRepositoryMock.Object);

            var logger = new Mock<ILogger>();
            
            _commandHandler = new CreateUserCommandHandler(_unitOfWorkMock.Object, logger.Object);

            _rightCreateCommandSample = new CreateUserCommand
            {
                SessionId = "ash1asdkj123"
            };

          

        }

        #region commands

        [TestMethod]
        public void Create_User_SessionId_Argument_Is_Null()
        {
            // Arrange
            _rightCreateCommandSample.SessionId = null;

            //Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _commandHandler.Execute(_rightCreateCommandSample));

            //Assert
            Assert.AreEqual("SessionId", result.ParamName);
            _userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public void Create_User_SessionId_Argument_Is_Empty()
        {
            // Arrange
            _rightCreateCommandSample.SessionId = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_rightCreateCommandSample));

            //Assert
            Assert.AreEqual("SessionId", result.ParamName);
            _userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public void Create_User_SessionId_Matches_Existing_User()
        {
            // Arrange
            _rightCreateCommandSample.SessionId = "someSessionId";

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _commandHandler.Execute(_rightCreateCommandSample));

            //Assert
            Assert.AreEqual("SessionId", result.ParamName);
            _userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public void Create_User_With_Right_Data()
        {
            // Arrange
            
            // Act
            _commandHandler.Execute(_rightCreateCommandSample);

            // Assert
            _userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.Save(), Times.Once);
        }
        #endregion

        #region queries
        #endregion
    }
}
