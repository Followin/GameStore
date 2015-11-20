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
        public CommandResult Execute(PerformPaymentCommand command)
        {
            var bankService = new BankServiceClient();
            var paymentInfo = new PaymentInfo
            {
                AccountNumber = command.Number,
                Name = command.Name,
                Sum = command.Sum,
                Cvv2 = command.Cvv2,
                ExpirationMonth = command.ExpirationMonth,
                ExpirationYear = command.ExpirationYear,
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
