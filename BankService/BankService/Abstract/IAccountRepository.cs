using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankService.Entities;

namespace BankService.Abstract
{
    public interface IAccountRepository
    {
        Task<Account> Get(String owner);

        void Add(Account account);

        void Delete(Int32 account);
    }
}
