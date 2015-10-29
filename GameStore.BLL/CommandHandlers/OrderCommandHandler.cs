using System;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.Commands;
using GameStore.BLL.Commands.Order;
using GameStore.BLL.CQRS;
using GameStore.BLL.Utils;
using GameStore.BLL.Utils.ValidationExtensions;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;

namespace GameStore.BLL.CommandHandlers
{
    public class OrderCommandHandler : 
    #region interfaces
        ICommandHandler<CreateOrderDetailsCommand>
    #endregion
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public OrderCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
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
            command.Discount.Argument(NameGetter.GetName(() => command.Discount))
                            .GreaterThan(-1);
            command.GameId.Argument(NameGetter.GetName(() => command.GameId))
                          .GreaterThan(0);
            command.OrderId.Argument(NameGetter.GetName(() => command.OrderId))
                           .GreaterThan(0);
            command.Price.Argument(NameGetter.GetName(() => command.Price))
                         .GreaterThan(-1);
            command.Quantity.Argument(NameGetter.GetName(() => command.Quantity))
                            .GreaterThan(0);
            if (_db.Games.Get(command.GameId) == null)
            {
                throw new ArgumentOutOfRangeException(
                    NameGetter.GetName(() => command.GameId),
                    "Game not found");
            }

            if (_db.Orders.Get(command.OrderId) == null)
            {
                throw new ArgumentOutOfRangeException(
                    NameGetter.GetName(() => command.OrderId),
                    "Order not found");
            }
        }
        #endregion
    }
}
