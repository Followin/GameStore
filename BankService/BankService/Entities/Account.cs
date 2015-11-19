using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankService.Entities
{
    public class Account
    {
        public Int32 Id { get; set; }

        public String Owner { get; set; }

        public Decimal Balance { get; set; }
    }
}