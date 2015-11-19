using System;
using GameStore.BLL.BankService;
using GameStore.BLL.Commands.Order;
using GameStore.BLL.CQRS;
using GameStore.DAL.Abstract;
using GameStore.Static;
using NLog;

namespace GameStore.BLL.CommandHandlers.Order
{
    public class PerformPaymentCommandHandler : ICommandHandler<PerformPaymentCommand>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public PerformPaymentCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public CommandResult Execute(PerformPaymentCommand command)
        {
            var bankService = new BankServiceClient();
            command.ExpirationDate = new DateTime(command.ExpirationDate.Year, command.ExpirationDate.Month, command.ExpirationDate.Day);
            var paymentInfo = new PaymentInfo
            {
                AccountNumber = command.Number,
                Name = command.Name,
                Sum = command.Sum,
                Cvv2 = command.Cvv2,
                ExpirationDate = command.ExpirationDate,
                Owner = "GameStore"
            };


            switch (command.Method)
            {
                case PaymentMethod.Visa:
                    var task = bankService.PayByVisaAsync(paymentInfo);
                    task.Wait();
                    return new CommandResult {Data = task.Result};
                case PaymentMethod.Mastercard:
                    task = bankService.PayByMastercardAsync(paymentInfo);
                    task.Wait();
                    return new CommandResult {Data = task.Result};
            }

            return new CommandResult {Success = false};
        }
    }
}
