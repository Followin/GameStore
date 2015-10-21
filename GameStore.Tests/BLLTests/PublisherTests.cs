using System;
using System.Linq;
using System.Linq.Expressions;
using GameStore.BLL.CommandHandlers;
using GameStore.BLL.Commands;
using GameStore.BLL.Queries;
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
    public class PublisherTests
    {
        private PublisherQueryHandler queryHandler;
        private PublisherCommandHandler commandHandler;
        private Mock<IRepository<Publisher, Int32>> publisherRepositoryMock;
        private Mock<IGameStoreUnitOfWork> unitOfWorkMock;
        private GetPublisherByCompanyNameQuery getPublisherByCompanyNameQuerySample;
        private CreatePublisherCommand createPublisherCommandSample;

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
            publisherRepositoryMock = new Mock<IRepository<Publisher, int>>();
            publisherRepositoryMock.Setup(x => x.GetSingle(It.IsAny<Expression<Func<Publisher, Boolean>>>())).Returns(
                (Expression<Func<Publisher, Boolean>> predicate) => publishers.FirstOrDefault(predicate.Compile()));

            unitOfWorkMock = new Mock<IGameStoreUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Publishers).Returns(publisherRepositoryMock.Object);

            var logger = new Mock<ILogger>();

            queryHandler = new PublisherQueryHandler(unitOfWorkMock.Object, logger.Object);
            commandHandler = new PublisherCommandHandler(unitOfWorkMock.Object, logger.Object);

            getPublisherByCompanyNameQuerySample = new GetPublisherByCompanyNameQuery
            {
                CompanyName = "Valve"
            };

            createPublisherCommandSample = new CreatePublisherCommand
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
            getPublisherByCompanyNameQuerySample.CompanyName = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(() =>
                queryHandler.Retrieve(getPublisherByCompanyNameQuerySample));

            // Assert
            unitOfWorkMock.Verify(x => x.Publishers, Times.Never);
            Assert.AreEqual("CompanyName", result.ParamName);
        }

        [TestMethod]
        public void GetPublisherByCompanyName_CompanyName_Argument_Is_Empty()
        {
            // Arrange
            getPublisherByCompanyNameQuerySample.CompanyName = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(() =>
                queryHandler.Retrieve(getPublisherByCompanyNameQuerySample));

            // Assert
            unitOfWorkMock.Verify(x => x.Publishers, Times.Never);
            Assert.AreEqual("CompanyName", result.ParamName);
        }

        [TestMethod]
        public void GetPublisherByCompanyName_With_Rigth_Data()
        {
            // Arrange
            // Act
            var result = queryHandler.Retrieve(getPublisherByCompanyNameQuerySample);

            // Assert
            Assert.AreEqual("Valve", result.CompanyName);
        }

        #endregion

        #region Create Publisher Tests

        [TestMethod]
        public void Create_Publisher_CompanyName_Argument_Is_Null()
        {
            // Arrange
            createPublisherCommandSample.CompanyName = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(
                () => commandHandler.Execute(createPublisherCommandSample));

            // Assert
            publisherRepositoryMock.Verify(x => x.Add(It.IsAny<Publisher>()), Times.Never);
            Assert.AreEqual("CompanyName", result.ParamName);
        }

        [TestMethod]
        public void Create_Publisher_CompanyName_Argument_Is_Empty()
        {
            // Arrange
            createPublisherCommandSample.CompanyName = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(
                () => commandHandler.Execute(createPublisherCommandSample));

            // Assert
            publisherRepositoryMock.Verify(x => x.Add(It.IsAny<Publisher>()), Times.Never);
            Assert.AreEqual("CompanyName", result.ParamName);
        }

        [TestMethod]
        public void Create_Publisher_CompanyName_Matches_Existing_Company()
        {
            // Arrange
            createPublisherCommandSample.CompanyName = "Valve";

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(
                () => commandHandler.Execute(createPublisherCommandSample));

            // Assert
            publisherRepositoryMock.Verify(x => x.Add(It.IsAny<Publisher>()), Times.Never);
            Assert.AreEqual("CompanyName", result.ParamName);
        }

        [TestMethod]
        public void Create_Publisher_Description_Argument_Is_Null()
        {
            // Arrange
            createPublisherCommandSample.Description = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(
                () => commandHandler.Execute(createPublisherCommandSample));

            // Assert
            publisherRepositoryMock.Verify(x => x.Add(It.IsAny<Publisher>()), Times.Never);
            Assert.AreEqual("Description", result.ParamName);
        }

        [TestMethod]
        public void Create_Publisher_Description_Argument_Is_Empty()
        {
            // Arrange
            createPublisherCommandSample.Description = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(
                () => commandHandler.Execute(createPublisherCommandSample));

            // Assert
            publisherRepositoryMock.Verify(x => x.Add(It.IsAny<Publisher>()), Times.Never);
            Assert.AreEqual("Description", result.ParamName);
        }

        [TestMethod]
        public void Create_Publisher_HomePage_Argument_Is_Null()
        {
            // Arrange
            createPublisherCommandSample.HomePage = null;

            // Act
            var result = ExceptionAssert.Throws<ArgumentNullException>(
                () => commandHandler.Execute(createPublisherCommandSample));

            // Assert
            publisherRepositoryMock.Verify(x => x.Add(It.IsAny<Publisher>()), Times.Never);
            Assert.AreEqual("HomePage", result.ParamName);
        }

        [TestMethod]
        public void Create_Publisher_HomePage_Argument_Is_Empty()
        {
            // Arrange
            createPublisherCommandSample.HomePage = String.Empty;

            // Act
            var result = ExceptionAssert.Throws<ArgumentException>(
                () => commandHandler.Execute(createPublisherCommandSample));

            // Assert
            publisherRepositoryMock.Verify(x => x.Add(It.IsAny<Publisher>()), Times.Never);
            Assert.AreEqual("HomePage", result.ParamName);
        }

        [TestMethod]
        public void Create_Publisher_With_Right_Data()
        {
            // Arrange
            // Act
            commandHandler.Execute(createPublisherCommandSample);

            // Assert
            publisherRepositoryMock.Verify(x => x.Add(It.Is<Publisher>(p => p.CompanyName == "Bethesda")), Times.Once);
            unitOfWorkMock.Verify(x => x.Save(),Times.Once);
        }
        #endregion
    }
}
