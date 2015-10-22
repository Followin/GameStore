using System;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.Commands;
using GameStore.BLL.CQRS;
using GameStore.BLL.Utils.ValidationExtensions;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;

namespace GameStore.BLL.CommandHandlers
{
    public class OrderCommandHandler : 
        ICommandHandler<CreateOrderDetailsCommand>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public OrderCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            this._db = db;
            this._logger = logger;
        }

        public void Execute(CreateOrderDetailsCommand command)
        {
            Validate(command);
            var orderDetails = Mapper.Map<CreateOrderDetailsCommand, OrderDetails>(command);
            _db.OrderDetails.Add(orderDetails);
            _db.Save();
        }

        #region validation

        private void Validate(CreateOrderDetailsCommand command)
        {
            command.Discount.Argument("Discount")
                            .GreaterThan(-1);
            command.GameId.Argument("GameId")
                          .GreaterThan(0);
            command.OrderId.Argument("OrderId")
                           .GreaterThan(0);
            command.Price.Argument("Price")
                         .GreaterThan(-1);
            command.Quantity.Argument("Quantity")
                            .GreaterThan(0);
            if (_db.Games.Get(command.GameId) == null)
            {
                throw new ArgumentOutOfRangeException("GameId", "Game not found");
            }

            if (_db.Orders.Get(command.OrderId) == null)
            {
                throw new ArgumentOutOfRangeException("OrderId", "Order not found");
            }
        }
        #endregion
    }
}
