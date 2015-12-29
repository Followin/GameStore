using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using GameStore.BLL.CommandHandlers;
using GameStore.BLL.CommandHandlers.Order;
using GameStore.BLL.Commands;
using GameStore.BLL.Commands.Order;
using GameStore.BLL.Concrete;
using GameStore.BLL.Observer;
using GameStore.DAL.Abstract;
using GameStore.DAL.Abstract.Repositories;
using GameStore.Domain.Entities;
using GameStore.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;

namespace GameStore.Tests.BLLTests
{
    [TestClass]
    public class OrderTests
    {
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<IGameRepository> _gameRepositoryMock;
        private Mock<IGameStoreUnitOfWork> _unitOfWorkMock;
        private CreateOrderDetailsCommandHandler _createOrderDetailsCommandHandler;
        private CheckoutOrderCommandHandler _checkoutOrderCommandHandler;
        private ShipOrderCommandHandler _shipOrderCommandHandler;
        private CreateOrderDetailsCommand _rightCreateOrderCommandSample;

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
                DescriptionEn = "Just try it",
                Key = "dota-2",
                Discontinued = false,
                UnitsInStock = 50,
                Price = 100
            };
            _gameRepositoryMock = new Mock<IGameRepository>();
            _gameRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(
                (int i) => i == 1 ? dota : null);

            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(
                (int i) => i == 1 ? new Order {OrderDetails = new List<OrderDetails>()}: null);


            _unitOfWorkMock = new Mock<IGameStoreUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.Orders).Returns(_orderRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.Games).Returns(_gameRepositoryMock.Object);

            _rightCreateOrderCommandSample = new CreateOrderDetailsCommand
            {
                Discount = 2.5F,
                GameId = 1,
                Price = 2.5M,
                Quantity = 2,
                OrderId = 1
            };

            var logger = new Mock<ILogger>();
            _createOrderDetailsCommandHandler = new CreateOrderDetailsCommandHandler(_unitOfWorkMock.Object, logger.Object);
            _checkoutOrderCommandHandler = new CheckoutOrderCommandHandler(_unitOfWorkMock.Object, logger.Object, new OrderNotificationSiren(new MessageSender(), _unitOfWorkMock.Object));
            _shipOrderCommandHandler = new ShipOrderCommandHandler(_unitOfWorkMock.Object, logger.Object);
        }

        #region Create tests

        [TestMethod]
        public void Create_Order_Details_Price_Argument_Is_Negative()
        {
            // Arrange
            _rightCreateOrderCommandSample.Price = -1;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _createOrderDetailsCommandHandler.Execute(_rightCreateOrderCommandSample));

            // Assert
            _orderRepositoryMock.Verify(x => x.AddOrderDetails(It.IsAny<OrderDetails>()), Times.Never);
            Assert.AreEqual("Price", result.ParamName);
        }

        [TestMethod]
        public void Create_Order_Discount_Argument_Is_Negative()
        {
            // Arrange
            _rightCreateOrderCommandSample.Discount = -1;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _createOrderDetailsCommandHandler.Execute(_rightCreateOrderCommandSample));

            // Assert
            _orderRepositoryMock.Verify(x => x.AddOrderDetails(It.IsAny<OrderDetails>()), Times.Never);
            Assert.AreEqual("Discount", result.ParamName);
        }

        [TestMethod]
        public void Create_Order_Quantity_Argument_Is_Zero()
        {
            // Arrange
            _rightCreateOrderCommandSample.Quantity = 0;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _createOrderDetailsCommandHandler.Execute(_rightCreateOrderCommandSample));

            // Assert
            _orderRepositoryMock.Verify(x => x.AddOrderDetails(It.IsAny<OrderDetails>()), Times.Never);
            Assert.AreEqual("Quantity", result.ParamName);
        }

        [TestMethod]
        public void Create_Order_Quantity_Argument_Is_Negative()
        {
            // Arrange
            _rightCreateOrderCommandSample.Quantity = -1;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _createOrderDetailsCommandHandler.Execute(_rightCreateOrderCommandSample));

            // Assert
            _orderRepositoryMock.Verify(x => x.AddOrderDetails(It.IsAny<OrderDetails>()), Times.Never);
            Assert.AreEqual("Quantity", result.ParamName);
        }

        [TestMethod]
        public void Create_Order_GameId_Argument_Is_Zero()
        {
            // Arrange
            _rightCreateOrderCommandSample.GameId = 0;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _createOrderDetailsCommandHandler.Execute(_rightCreateOrderCommandSample));

            // Assert
            _orderRepositoryMock.Verify(x => x.AddOrderDetails(It.IsAny<OrderDetails>()), Times.Never);
            Assert.AreEqual("GameId", result.ParamName);
        }

        [TestMethod]
        public void Create_Order_GameId_Argument_Is_Negative()
        {
            // Arrange
            _rightCreateOrderCommandSample.GameId = -1;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _createOrderDetailsCommandHandler.Execute(_rightCreateOrderCommandSample));

            // Assert
            _orderRepositoryMock.Verify(x => x.AddOrderDetails(It.IsAny<OrderDetails>()), Times.Never);
            Assert.AreEqual("GameId", result.ParamName);
        }

        [TestMethod]
        public void Create_Order_GameId_Argument_Doesnt_Match_Existing_Game()
        {
            // Arrange
            _rightCreateOrderCommandSample.GameId = 100;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _createOrderDetailsCommandHandler.Execute(_rightCreateOrderCommandSample));

            // Assert
            _orderRepositoryMock.Verify(x => x.AddOrderDetails(It.IsAny<OrderDetails>()), Times.Never);
            Assert.AreEqual("GameId", result.ParamName);
        }

        [TestMethod]
        public void Create_Order_OrderId_Argument_Is_Zero()
        {
            // Arrange
            _rightCreateOrderCommandSample.OrderId = 0;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _createOrderDetailsCommandHandler.Execute(_rightCreateOrderCommandSample));

            // Assert
            _orderRepositoryMock.Verify(x => x.AddOrderDetails(It.IsAny<OrderDetails>()), Times.Never);
            Assert.AreEqual("OrderId", result.ParamName);
        }

        [TestMethod]
        public void Create_Order_OrderId_Argument_Is_Negative()
        {
            // Arrange
            _rightCreateOrderCommandSample.OrderId = -1;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _createOrderDetailsCommandHandler.Execute(_rightCreateOrderCommandSample));

            // Assert
            _orderRepositoryMock.Verify(x => x.AddOrderDetails(It.IsAny<OrderDetails>()), Times.Never);
            Assert.AreEqual("OrderId", result.ParamName);
        }

        [TestMethod]
        public void Create_Order_OrderId_Argument_Doesnt_Match_Existing_Game()
        {
            // Arrange
            _rightCreateOrderCommandSample.OrderId = 100;

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _createOrderDetailsCommandHandler.Execute(_rightCreateOrderCommandSample));

            // Assert
            _orderRepositoryMock.Verify(x => x.AddOrderDetails(It.IsAny<OrderDetails>()), Times.Never);
            Assert.AreEqual("OrderId", result.ParamName);
        }

        [TestMethod]
        public void Create_OrderDetails_With_Right_Data()
        {
            // Arrange
            // Act
            _createOrderDetailsCommandHandler.Execute(_rightCreateOrderCommandSample);

            // Assert
            _orderRepositoryMock.Verify(x => x.AddOrderDetails(It.IsAny<OrderDetails>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.Save(), Times.Once);
        }

        #endregion

        #region checkout

        [TestMethod]
        public void CheckoutOrder_Id_Argument_Is_Zero()
        {
            // Arrange
            var checkoutOrderCommand = new CheckoutOrderCommand() { Id = 0 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _checkoutOrderCommandHandler.Execute(checkoutOrderCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Save(), Times.Never);
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void CheckoutOrder_Id_Argument_Is_Negative()
        {
            // Arrange
            var checkoutOrderCommand = new CheckoutOrderCommand { Id = -1 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _checkoutOrderCommandHandler.Execute(checkoutOrderCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Save(), Times.Never);
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void CheckoutOrder_Id_Argument_Doesnt_Match_Existing_Order()
        {
            // Arrange
            var checkoutOrderCommand = new CheckoutOrderCommand { Id = 100 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _checkoutOrderCommandHandler.Execute(checkoutOrderCommand));

            // Assert
            _unitOfWorkMock.Verify(x => x.Save(), Times.Never);
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void CheckoutOrder_Right_Data()
        {
            // Arrange
            var checkoutOrderCommand = new CheckoutOrderCommand { Id = 1 };

            // Act
            _checkoutOrderCommandHandler.Execute(checkoutOrderCommand);

            // Assert
            _unitOfWorkMock.Verify(x => x.Save(), Times.Once);
        }

        #endregion

        #region shipOrder

        [TestMethod]
        public void ShipOrder_Id_Argument_Is_Zero()
        {
            // Arrange
            var shipOrderCommandHandler = new ShipOrderCommand { Id = 0 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _shipOrderCommandHandler.Execute(shipOrderCommandHandler));

            // Assert
            _unitOfWorkMock.Verify(x => x.Save(), Times.Never);
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void ShipOrder_Id_Argument_Is_Negative()
        {
            // Arrange
            var shipOrderCommandHandler = new ShipOrderCommand { Id = -1 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _shipOrderCommandHandler.Execute(shipOrderCommandHandler));

            // Assert
            _unitOfWorkMock.Verify(x => x.Save(), Times.Never);
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void ShipOrder_Id_Argument_Doesnt_Match_Existing_Order()
        {
            // Arrange
            var shipOrderCommandHandler = new ShipOrderCommand { Id = 100 };

            // Act
            var result = ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                _shipOrderCommandHandler.Execute(shipOrderCommandHandler));

            // Assert
            _unitOfWorkMock.Verify(x => x.Save(), Times.Never);
            Assert.AreEqual("Id", result.ParamName);
        }

        [TestMethod]
        public void ShipOrder_Right_Data()
        {
            // Arrange
            var shipOrderCommandHandler = new ShipOrderCommand { Id = 1 };

            // Act
            _shipOrderCommandHandler.Execute(shipOrderCommandHandler);

            // Assert
            _unitOfWorkMock.Verify(x => x.Save(), Times.Once);
        }

        #endregion
    }
}
