using System;
using System.Threading.Tasks;
using BankService.Entities;

namespace BankService.Abstract
{
    public interface ITransferRepository
    {
        Task AddTransfer(Transfer transfer);

        Task<Transfer> GetTransfer(Func<Transfer, Boolean> predicate);
    }
}