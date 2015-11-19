using System;

namespace BankService.Entities
{
    public class Transfer
    {
        public Int32 Id { get; set; }

        public Account Initiator { get; set; }

        public User User { get; set; }

        public Decimal Sum { get; set; }

        public TransferType Type { get; set; }

        public String VerificationCode { get; set; }

        public DateTime InitTime { get; set; }

        public DateTime? PayTime { get; set; }
    }
}