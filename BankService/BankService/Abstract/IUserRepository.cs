using System;
using System.Threading.Tasks;
using BankService.Entities;

namespace BankService.Abstract
{
    public interface IUserRepository
    {
        Task<User> Get(String name, String accountNumber, CardType cardType, DateTime expirationDate, String cvv);

        void Add(Int32 id);

        void Delete(Int32 id);
    }
}
