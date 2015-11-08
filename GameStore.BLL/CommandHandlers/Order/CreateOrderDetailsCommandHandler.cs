﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.Commands.Order;
using GameStore.BLL.CQRS;
using GameStore.BLL.Utils;
using GameStore.BLL.Utils.ValidationExtensions;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;

namespace GameStore.BLL.CommandHandlers.Order
{
    public class CreateOrderDetailsCommandHandler : ICommandHandler<CreateOrderDetailsCommand>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public CreateOrderDetailsCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public CommandResult Execute(CreateOrderDetailsCommand command)
        {
            Validate(command);
            var order = _db.Orders.Get(command.OrderId);
            var existingOrderDetails = order.OrderDetails.FirstOrDefault(x => x.GameId == command.GameId);
            if (existingOrderDetails != null)
            {
                existingOrderDetails.Quantity += command.Quantity;
                _db.Orders.EditOrderDetails(existingOrderDetails);
            }
            else
            {
                var orderDetails = Mapper.Map<CreateOrderDetailsCommand, OrderDetails>(command);
                _db.Orders.AddOrderDetails(orderDetails);
            }

            _db.Save();

            return new CommandResult();
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

        }
        #endregion
    }
}
