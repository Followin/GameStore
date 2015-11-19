using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.Commands.Order;
using GameStore.BLL.CQRS;
using GameStore.DAL.Abstract;
using NLog;

namespace GameStore.BLL.CommandHandlers.Order
{
    public class ConfirmPaymentCommandHandler : ICommandHandler<ConfirmPaymentCommand>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public ConfirmPaymentCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public CommandResult Execute(ConfirmPaymentCommand command)
        {
            var bankService = new BankService.BankServiceClient();

            var task = bankService.ConfirmAsync(command.Code);
            task.Wait();

            return new CommandResult {Success = task.Result};
        }
    }
}
