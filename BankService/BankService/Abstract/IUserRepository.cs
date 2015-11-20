using System;
using System.Threading.Tasks;
using BankService.Entities;

namespace BankService.Abstract
{
    public interface IUserRepository
    {
        Task<User> Get(String name, String accountNumber, CardType cardType, String cvv, Int32 expirationMonth, Int32 expirationYear);

        void Add(Int32 id);

        void Delete(Int32 id);
    }
}
