using System;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using GameStore.BLL.Commands.Order;
using GameStore.BLL.CQRS;
using GameStore.BLL.Utils;
using GameStore.Domain.Abstract;
using NLog;

namespace GameStore.BLL.CommandHandlers.Order
{
    public class CheckoutOrderCommandHandler : ICommandHandler<CheckoutOrderCommand>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public CheckoutOrderCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public CommandResult Execute(CheckoutOrderCommand command)
        {
            command.Id.Argument(NameGetter.GetName(() => command.Id))
                   .GreaterThan(0);

            var order = _db.Orders.Get(command.Id);
            if (order == null)
            {
                throw new ArgumentOutOfRangeException(
                    NameGetter.GetName(() => command.Id), "Order not found");
            }

            try
            {
                _db.Orders.Checkout(command.Id);
                _db.Save();
                return new CommandResult();
            }
            catch
            {
                return new CommandResult {Success = false};
            }
        }
    }
}
