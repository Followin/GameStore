using System;
using GameStore.BLL.Commands.Order;
using GameStore.BLL.CQRS;
using GameStore.DAL.Abstract;
using NLog;

namespace GameStore.BLL.CommandHandlers.Order
{
    public class DeleteOrderDetailsCommandHandler : ICommandHandler<DeleteOrderDetailsCommand>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public DeleteOrderDetailsCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public CommandResult Execute(DeleteOrderDetailsCommand command)
        {
            try
            {
                _db.Orders.DeleteOrderDetails(command.GameId, command.OrderId);
                _db.Save();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
                return new CommandResult {Success = false};
            }
            return new CommandResult();
        }
    }
}
