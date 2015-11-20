using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BankService.Abstract;
using BankService.Entities;

namespace BankService.Concrete
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> Get(string name, string accountNumber, CardType cardType, string cvv, Int32 expirationMonth, Int32 expirationYear)
        {
            return
                Database.Users.Find(
                    x => x.Name+" "+x.Surname == name && x.AccountNumber == accountNumber && x.CardType == cardType && x.ExpirationMonth == expirationMonth && x.ExpirationYear == expirationYear && x.Cvv2 == cvv);
        }

        public void Add(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}