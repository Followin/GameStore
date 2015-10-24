using System;
using System.Linq;
using System.Linq.Expressions;
using GameStore.BLL.CommandHandlers;
using GameStore.BLL.Commands;
using GameStore.BLL.Commands.Publisher;
using GameStore.BLL.Queries;
using GameStore.BLL.Queries.Publisher;
using GameStore.BLL.QueryHandlers;
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
    public class PublisherTests
    {
        private PublisherQueryHandler _queryHandler;
        private PublisherCommandHandler _commandHandler;
        private Mock<IPublisherRepository> _publisherRepositoryMock;
        private Mock<IGameStoreUnitOfWork> _unitOfWorkMock;
        private GetPublisherByCompanyNameQuery _getPublisherByCompanyNameQuerySample;
        private CreatePublisherCommand _createPublisherCommandSample;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            AutoMapperConfiguration.Configure();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var valve = new Publisher
            {
                Id = 1,
                CompanyName = "Valve",
                Description = "Greed Gaben's company",
                HomePage = "http://www.valvesoftware.com/",
            };
            var cdProject = new Publisher
            {
                Id = 2,
                CompanyName = "CD Project",
                Description = "Poland private game developing company",
                HomePage = "https://www.cdprojekt.com/"
            };
            var publishers = new[] { valve, cdProject };
            _publisherRepositoryMock = new Mock<IPublisherRepository>();
            _publisherRepositoryMock.Setup(x => x.GetSingle(It.IsAny<Expression<Func<Publisher, Boolean>>>())).Returns(
                (Expression<Func<Publisher, Boolean>> predicate) => publishers.FirstOrDefault(predicate.Compile()));

            _unitOfWorkMock = new Mock<IGameStoreUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.Publishers).Returns(_publisherRepositoryMock.Object);

            var logger = new Mock<ILogger>();

            _queryHandler = new PublisherQueryHandler(_unitOfWorkMock.Object, logger.Object);
            _commandHandler = new PublisherCommandHandler(_unitOfWorkMock.Object, logger.Object);

            _getPublisherByCompanyNameQuerySample = new GetPublisherByCompanyNameQuery
            {
                CompanyName = "Valve"
            };

            _createPublisherCommandSample = new CreatePublisherCommand
            {
                CompanyName = "Bethesda",
                Description = "American video game publisher. A subsidiary of ZeniMax Media.",
                HomePage = "http://bethsoft.com/"
            };
        }

        #region GetTests

        [TestMethod]
        public void GetPublisherByCompanyName_CompanyName_Argument_Is_Null()
        {
            // Arrange
            _getPublisherByCompanyNameQuerySample.CompanyName = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                _queryHandler.Retrieve(_getPublisherByCompanyNameQuerySample));

            // Assert
            _unitOfWorkMock.Verify(x => x.Publishers, Times.Never);
            Assert.AreEqual("CompanyName", result.ParamName);
        }

        [TestMethod]
        public void GetPublisherByCompanyName_CompanyName_Argument_Is_Empty()
        {
            // Arrange
            _getPublisherByCompanyNameQuerySample.CompanyName = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                _queryHandler.Retrieve(_getPublisherByCompanyNameQuerySample));

            // Assert
            _unitOfWorkMock.Verify(x => x.Publishers, Times.Never);
            Assert.AreEqual("CompanyName", result.ParamName);
        }

        [TestMethod]
        public void GetPublisherByCompanyName_With_Rigth_Data()
        {
            // Arrange
            // Act
            var result = _queryHandler.Retrieve(_getPublisherByCompanyNameQuerySample);

            // Assert
            Assert.AreEqual("Valve", result.CompanyName);
        }

        #endregion

        #region Create Publisher Tests

        [TestMethod]
        public void Create_Publisher_CompanyName_Argument_Is_Null()
        {
            // Arrange
            _createPublisherCommandSample.CompanyName = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(
                () => _commandHandler.Execute(_createPublisherCommandSample));

            // Assert
            _publisherRepositoryMock.Verify(x => x.Add(It.IsAny<Publisher>()), Times.Never);
            Assert.AreEqual("CompanyName", result.ParamName);
        }

        [TestMethod]
        public void Create_Publisher_CompanyName_Argument_Is_Empty()
        {
            // Arrange
            _createPublisherCommandSample.CompanyName = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(
                () => _commandHandler.Execute(_createPublisherCommandSample));

            // Assert
            _publisherRepositoryMock.Verify(x => x.Add(It.IsAny<Publisher>()), Times.Never);
            Assert.AreEqual("CompanyName", result.ParamName);
        }

        [TestMethod]
        public void Create_Publisher_CompanyName_Matches_Existing_Company()
        {
            // Arrange
            _createPublisherCommandSample.CompanyName = "Valve";

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(
                () => _commandHandler.Execute(_createPublisherCommandSample));

            // Assert
            _publisherRepositoryMock.Verify(x => x.Add(It.IsAny<Publisher>()), Times.Never);
            Assert.AreEqual("CompanyName", result.ParamName);
        }

        [TestMethod]
        public void Create_Publisher_Description_Argument_Is_Null()
        {
            // Arrange
            _createPublisherCommandSample.Description = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(
                () => _commandHandler.Execute(_createPublisherCommandSample));

            // Assert
            _publisherRepositoryMock.Verify(x => x.Add(It.IsAny<Publisher>()), Times.Never);
            Assert.AreEqual("Description", result.ParamName);
        }

        [TestMethod]
        public void Create_Publisher_Description_Argument_Is_Empty()
        {
            // Arrange
            _createPublisherCommandSample.Description = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(
                () => _commandHandler.Execute(_createPublisherCommandSample));

            // Assert
            _publisherRepositoryMock.Verify(x => x.Add(It.IsAny<Publisher>()), Times.Never);
            Assert.AreEqual("Description", result.ParamName);
        }

        [TestMethod]
        public void Create_Publisher_HomePage_Argument_Is_Null()
        {
            // Arrange
            _createPublisherCommandSample.HomePage = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(
                () => _commandHandler.Execute(_createPublisherCommandSample));

            // Assert
            _publisherRepositoryMock.Verify(x => x.Add(It.IsAny<Publisher>()), Times.Never);
            Assert.AreEqual("HomePage", result.ParamName);
        }

        [TestMethod]
        public void Create_Publisher_HomePage_Argument_Is_Empty()
        {
            // Arrange
            _createPublisherCommandSample.HomePage = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(
                () => _commandHandler.Execute(_createPublisherCommandSample));

            // Assert
            _publisherRepositoryMock.Verify(x => x.Add(It.IsAny<Publisher>()), Times.Never);
            Assert.AreEqual("HomePage", result.ParamName);
        }

        [TestMethod]
        public void Create_Publisher_With_Right_Data()
        {
            // Arrange
            // Act
            _commandHandler.Execute(_createPublisherCommandSample);

            // Assert
            _publisherRepositoryMock.Verify(x => x.Add(It.Is<Publisher>(p => p.CompanyName == "Bethesda")), Times.Once);
            _unitOfWorkMock.Verify(x => x.Save(), Times.Once);
        }
        #endregion
    }
}
