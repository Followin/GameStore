using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using GameStore.BLL.Commands.Order;
using GameStore.BLL.CQRS;
using GameStore.BLL.Utils;
using GameStore.DAL.Abstract;
using NLog;

namespace GameStore.BLL.CommandHandlers.Order
{
    public class ShipOrderCommandHandler : ICommandHandler<ShipOrderCommand>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public ShipOrderCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public CommandResult Execute(ShipOrderCommand command)
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
                var date = _db.Orders.Ship(command.Id);
                _db.Save();
                return new CommandResult { Data = date };
                
            }
            catch
            {
                return new CommandResult {Success = false};
            }

        }


    }
}
