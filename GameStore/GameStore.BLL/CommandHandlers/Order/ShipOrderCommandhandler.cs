using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.Commands.Order;
using GameStore.BLL.CQRS;
using GameStore.Domain.Abstract;

namespace GameStore.BLL.CommandHandlers.Order
{
    public class ShipOrderCommandHandler : ICommandHandler<ShipOrderCommand>
    {
        private IGameStoreUnitOfWork _db;

        public ShipOrderCommandHandler(IGameStoreUnitOfWork db)
        {
            _db = db;
        }

        public CommandResult Execute(ShipOrderCommand command)
        {
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
