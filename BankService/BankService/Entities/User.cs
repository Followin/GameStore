using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankService.Entities
{
    public class User
    {
        public Int32 Id { get; set; }

        public String Name { get; set; }

        public String Surname { get; set; }

        public String AccountNumber { get; set; }

        public String Cvv2 { get; set; }

        public DateTime ExpirationDate { get; set; }

        public String Email { get; set; }

        public Decimal Balance { get; set; }

        public CardType CardType { get; set; }

        public String PhoneNumber { get; set; }
    }
}