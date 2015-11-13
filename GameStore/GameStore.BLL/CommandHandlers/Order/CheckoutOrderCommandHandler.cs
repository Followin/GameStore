using GameStore.BLL.Commands.Order;
using GameStore.BLL.CQRS;
using GameStore.Domain.Abstract;

namespace GameStore.BLL.CommandHandlers.Order
{
    public class CheckoutOrderCommandHandler : ICommandHandler<CheckoutOrderCommand>
    {
        private IGameStoreUnitOfWork _db;

        public CheckoutOrderCommandHandler(IGameStoreUnitOfWork db)
        {
            _db = db;
        }

        public CommandResult Execute(CheckoutOrderCommand command)
        {
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
