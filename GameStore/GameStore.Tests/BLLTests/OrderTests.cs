//using System;
//using System.Linq.Expressions;
//using System.Security.Cryptography.X509Certificates;
//using GameStore.BLL.CommandHandlers;
//using GameStore.BLL.Commands;
//using GameStore.BLL.Commands.Order;
//using GameStore.Domain.Abstract;
//using GameStore.Domain.Abstract.Repositories;
//using GameStore.Domain.Entities;
//using GameStore.Tests.Utils;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using NLog;

//namespace GameStore.Tests.BLLTests
//{
//    [TestClass]
//    public class OrderTests
//    {
//        private Mock<IOrderRepository> _orderRepositoryMock;
//        private Mock<IGameRepository> _gameRepositoryMock;
//        private Mock<IGameStoreUnitOfWork> _unitOfWorkMock;
//        private OrderCommandHandler _commandHandler;
//        private CreateOrderDetailsCommand _rightCreateOrderCommandSample;

//        [ClassInitialize]
//        public static void ClassInitialize(TestContext context)
//        {
//            AutoMapperConfiguration.Configure();
//        }

//        [TestInitialize]
//        public void TestInitialize()
//        {
//            var dota = new Game
//            {
//                Id = 1,
//                Name = "Dota 2",
//                Description = "Just try it",
//                Key = "dota-2",
//                Discontinued = false,
//                UnitsInStock = 50,
//                Price = 100
//            };
//            _gameRepositoryMock = new Mock<IGameRepository>();
//            _gameRepositoryMock.Setup(x => x.Get(It.IsAny<Int32>())).Returns(
//                (Int32 i) => i == 1 ? dota : null);

//            _orderRepositoryMock = new Mock<IOrderRepository>();
//            _orderRepositoryMock.Setup(x => x.Get(It.IsAny<Int32>())).Returns(
//                (Int32 i) => i == 1 ? new Order() : null);

//            _orderDetailsRepositoryMock = new Mock<IOrderDetailsRepository>();

//            _unitOfWorkMock = new Mock<IGameStoreUnitOfWork>();
//            _unitOfWorkMock.Setup(x => x.OrderDetails).Returns(_orderDetailsRepositoryMock.Object);
//            _unitOfWorkMock.Setup(x => x.Orders).Returns(_orderRepositoryMock.Object);
//            _unitOfWorkMock.Setup(x => x.Games).Returns(_gameRepositoryMock.Object);

//            _rightCreateOrderCommandSample = new CreateOrderDetailsCommand
//            {
//                Discount = 2.5F,
//                GameId = 1,
//                Price = 2.5M,
//                Quantity = 2,
//                OrderId = 1
//            };

//            var logger = new Mock<ILogger>();
//            _commandHandler = new OrderCommandHandler(_unitOfWorkMock.Object, logger.Object);
//        }

//        #region Create tests

//        [TestMethod]
//        public void Create_Order_Details_Price_Argument_Is_Negative()
//        {
//            // Arrange
//            _rightCreateOrderCommandSample.Price = -1;

//            // Act
//            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
//                _commandHandler.Execute(_rightCreateOrderCommandSample));

//            // Assert
//            _orderDetailsRepositoryMock.Verify(x => x.Add(It.IsAny<OrderDetails>()), Times.Never);
//            Assert.AreEqual("Price", result.ParamName);
//        }

//        [TestMethod]
//        public void Create_Order_Discount_Argument_Is_Negative()
//        {
//            // Arrange
//            _rightCreateOrderCommandSample.Discount = -1;

//            // Act
//            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
//                _commandHandler.Execute(_rightCreateOrderCommandSample));

//            // Assert
//            _orderDetailsRepositoryMock.Verify(x => x.Add(It.IsAny<OrderDetails>()), Times.Never);
//            Assert.AreEqual("Discount", result.ParamName);
//        }

//        [TestMethod]
//        public void Create_Order_Quantity_Argument_Is_Zero()
//        {
//            // Arrange
//            _rightCreateOrderCommandSample.Quantity = 0;

//            // Act
//            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
//                _commandHandler.Execute(_rightCreateOrderCommandSample));

//            // Assert
//            _orderDetailsRepositoryMock.Verify(x => x.Add(It.IsAny<OrderDetails>()), Times.Never);
//            Assert.AreEqual("Quantity", result.ParamName);
//        }

//        [TestMethod]
//        public void Create_Order_Quantity_Argument_Is_Negative()
//        {
//            // Arrange
//            _rightCreateOrderCommandSample.Quantity = -1;

//            // Act
//            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
//                _commandHandler.Execute(_rightCreateOrderCommandSample));

//            // Assert
//            _orderDetailsRepositoryMock.Verify(x => x.Add(It.IsAny<OrderDetails>()), Times.Never);
//            Assert.AreEqual("Quantity", result.ParamName);
//        }

//        [TestMethod]
//        public void Create_Order_GameId_Argument_Is_Zero()
//        {
//            // Arrange
//            _rightCreateOrderCommandSample.GameId = 0;

//            // Act
//            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
//                _commandHandler.Execute(_rightCreateOrderCommandSample));

//            // Assert
//            _orderDetailsRepositoryMock.Verify(x => x.Add(It.IsAny<OrderDetails>()), Times.Never);
//            Assert.AreEqual("GameId", result.ParamName);
//        }

//        [TestMethod]
//        public void Create_Order_GameId_Argument_Is_Negative()
//        {
//            // Arrange
//            _rightCreateOrderCommandSample.GameId = -1;

//            // Act
//            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
//                _commandHandler.Execute(_rightCreateOrderCommandSample));

//            // Assert
//            _orderDetailsRepositoryMock.Verify(x => x.Add(It.IsAny<OrderDetails>()), Times.Never);
//            Assert.AreEqual("GameId", result.ParamName);
//        }

//        [TestMethod]
//        public void Create_Order_GameId_Argument_Doesnt_Match_Existing_Game()
//        {
//            // Arrange
//            _rightCreateOrderCommandSample.GameId = 100;

//            // Act
//            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
//                _commandHandler.Execute(_rightCreateOrderCommandSample));

//            // Assert
//            _orderDetailsRepositoryMock.Verify(x => x.Add(It.IsAny<OrderDetails>()), Times.Never);
//            Assert.AreEqual("GameId", result.ParamName);
//        }

//        [TestMethod]
//        public void Create_Order_OrderId_Argument_Is_Zero()
//        {
//            // Arrange
//            _rightCreateOrderCommandSample.OrderId = 0;

//            // Act
//            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
//                _commandHandler.Execute(_rightCreateOrderCommandSample));

//            // Assert
//            _orderDetailsRepositoryMock.Verify(x => x.Add(It.IsAny<OrderDetails>()), Times.Never);
//            Assert.AreEqual("OrderId", result.ParamName);
//        }

//        [TestMethod]
//        public void Create_Order_OrderId_Argument_Is_Negative()
//        {
//            // Arrange
//            _rightCreateOrderCommandSample.OrderId = -1;

//            // Act
//            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
//                _commandHandler.Execute(_rightCreateOrderCommandSample));

//            // Assert
//            _orderDetailsRepositoryMock.Verify(x => x.Add(It.IsAny<OrderDetails>()), Times.Never);
//            Assert.AreEqual("OrderId", result.ParamName);
//        }

//        [TestMethod]
//        public void Create_Order_OrderId_Argument_Doesnt_Match_Existing_Game()
//        {
//            // Arrange
//            _rightCreateOrderCommandSample.OrderId = 100;

//            // Act
//            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
//                _commandHandler.Execute(_rightCreateOrderCommandSample));

//            // Assert
//            _orderDetailsRepositoryMock.Verify(x => x.Add(It.IsAny<OrderDetails>()), Times.Never);
//            Assert.AreEqual("OrderId", result.ParamName);
//        }

//        [TestMethod]
//        public void Create_Order_With_Right_Data()
//        {
//            // Arrange
//            // Act
//            _commandHandler.Execute(_rightCreateOrderCommandSample);

//            // Assert
//            _orderDetailsRepositoryMock.Verify(x => x.Add(It.Is<OrderDetails>(o => o.Price == 2.5M)));
//            _unitOfWorkMock.Verify(x => x.Save(), Times.Once);
//        }
//        #endregion
//    }
//}
