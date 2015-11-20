using System;
using System.Threading.Tasks;
using BankService.Abstract;
using BankService.Concrete;
using BankService.Entities;
using BankService.Models;

namespace BankService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BankService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BankService.svc or BankService.svc.cs at the Solution Explorer and start debugging.
    public class BankService : IBankService
    {
        private IUserRepository _userRepository = new UserRepository();
        private ITransferRepository _transferRepository = new TransferRepository();
        private IAccountRepository _accountRepository = new AccountRepository();
        private IMessageService _messageService = new MessageService();

        public async Task<PaymentResult> PayByVisaAsync(PaymentInfo info)
        {
            return await Pay(info, CardType.Visa);
        }

        public async Task<PaymentResult> PayByMastercardAsync(PaymentInfo info)
        {
            return await Pay(info, CardType.Mastercard);
        }

        public async Task<Boolean> ConfirmAsync(String code)
        {
            var transfer = await _transferRepository.GetTransfer(x => x.VerificationCode == code);
            if (transfer == null)
            {
                return false;
            }

            transfer.Initiator.Balance += transfer.Sum;
            transfer.User.Balance -= transfer.Sum;
            transfer.PayTime = DateTime.UtcNow;
            await _messageService.SendEmail(transfer.User.Email, "Transfer sum: " + transfer.Sum);

            return true;
        }


        private async Task<PaymentResult> Pay(PaymentInfo info, CardType type)
        {
            var user = await _userRepository.Get(info.Name, info.AccountNumber, type, info.Cvv2, info.ExpirationMonth, info.ExpirationYear);
            if (user == null)
            {
                return PaymentResult.CardDoesntExist;
            }

            if (user.Balance < info.Sum)
            {
                return PaymentResult.NotEnoughMoney;
            }

            var account = await _accountRepository.Get(info.Owner);

            var transfer = new Transfer
            {
                InitTime = DateTime.UtcNow,
                Initiator = account,
                Sum = info.Sum,
                Type = TransferType.Income,
                User = user
            };

            if (user.PhoneNumber == null)
            {
                transfer.PayTime = DateTime.UtcNow;
                user.Balance -= info.Sum;
                account.Balance += info.Sum;

                await _messageService.SendEmail(user.Email, "Transfer sum: " + transfer.Sum);

                await _transferRepository.AddTransfer(transfer);

                return PaymentResult.Success;
            }

            var random = new Random(DateTime.UtcNow.Millisecond);

            transfer.VerificationCode = random.Next(100, 1000000).ToString();
            await _messageService.SendSms(user.PhoneNumber, transfer.VerificationCode);
            await _transferRepository.AddTransfer(transfer);

            return PaymentResult.CodeConfirmRequired;
        }
    }
}
