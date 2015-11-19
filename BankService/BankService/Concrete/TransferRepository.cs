using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BankService.Abstract;
using BankService.Entities;

namespace BankService.Concrete
{
    public class TransferRepository : ITransferRepository
    {
        public async Task AddTransfer(Transfer transfer)
        {
            if (Database.Transfers.Any())
            {
                var lastId = Database.Transfers.Max(x => x.Id);
                transfer.Id = lastId + 1;
            }
            else
            {
                transfer.Id = 1;
            }
            Database.Transfers.Add(transfer);
        }

        public async Task<Transfer> GetTransfer(Func<Transfer, bool> predicate)
        {
            return Database.Transfers.FirstOrDefault(predicate);
        }
    }
}