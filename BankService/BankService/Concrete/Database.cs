using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankService.Entities;

namespace BankService.Concrete
{
    public static class Database
    {
        public static List<Account> Accounts { get; private set; }

        public static List<User> Users { get; private set; }

        public static List<Transfer> Transfers { get; private set; }

        static Database()
        {
            Accounts = new List<Account>
            {
                new Account {Id = 1, Balance = 25.5M, Owner = "GameStore"}
            };

            Users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Balance = 1000M,
                    Name = "First",
                    Surname = "User",
                    AccountNumber = "firstuseraccount.",
                    ExpirationDate = new DateTime(2000, 1, 1),
                    Email = "Dlike.version10@gmail.com",
                    Cvv2 = "123",
                    CardType = CardType.Visa,
                    PhoneNumber = "+380953044036"
                },

                new User
                {
                    Id = 2,
                    Balance = 200M,
                    Name = "Second",
                    Surname = "User",
                    AccountNumber = "seconduseraccount.",
                    ExpirationDate = new DateTime(2000, 1, 1),
                    Email = "angel124user@yahoo.com",
                    CardType = CardType.Mastercard,
                    Cvv2 = "123",
                    PhoneNumber = "12313123123"
                }
            };

            Transfers = new List<Transfer>();
        }
    }
}