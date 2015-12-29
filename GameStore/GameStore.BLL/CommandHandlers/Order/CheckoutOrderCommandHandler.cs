using System;
using System.Linq;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using GameStore.BLL.Commands.Order;
using GameStore.BLL.CQRS;
using GameStore.BLL.Observer;
using GameStore.BLL.Utils;
using GameStore.DAL.Abstract;
using NLog;

namespace GameStore.BLL.CommandHandlers.Order
{
    public class CheckoutOrderCommandHandler : ICommandHandler<CheckoutOrderCommand>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;
        private OrderNotificationSiren _notificationSiren;

        public CheckoutOrderCommandHandler(IGameStoreUnitOfWork db, ILogger logger, OrderNotificationSiren notificationSiren)
        {
            _db = db;
            _logger = logger;
            _notificationSiren = notificationSiren;
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
                
                _notificationSiren.Notify("someone just bought games for" + Math.Round(order.OrderDetails.Sum(x => x.Price * x.Quantity),2) + "$");

                return new CommandResult();
            }
            catch
            {
                return new CommandResult {Success = false};
            }
        }
    }
}
