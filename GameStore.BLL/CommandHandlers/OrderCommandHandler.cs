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
        private IGameStoreUnitOfWork db;
        private ILogger logger;

        public OrderCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public void Execute(CreateOrderDetailsCommand command)
        {
            Validate(command);
            var orderDetails = Mapper.Map<CreateOrderDetailsCommand, OrderDetails>(command);
            db.OrderDetails.Add(orderDetails);
            db.Save();
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
            if (db.Games.Get(command.GameId) == null)
            {
                throw new ArgumentOutOfRangeException("GameId", "Game not found");
            }

            if (db.Orders.Get(command.OrderId) == null)
            {
                throw new ArgumentOutOfRangeException("OrderId", "Order not found");
            }

        }
        #endregion
    }
}
