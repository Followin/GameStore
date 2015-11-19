using System;
using System.Threading.Tasks;
using BankService.Abstract;
using BankService.Entities;

namespace BankService.Concrete
{
    public class AccountRepository : IAccountRepository
    {
        public async Task<Account> Get(string owner)
        {
            return Database.Accounts.Find(x => x.Owner == owner);
        }

        public void Add(Account account)
        {
            throw new NotImplementedException();
        }

        public void Delete(int account)
        {
            throw new NotImplementedException();
        }
    }
}