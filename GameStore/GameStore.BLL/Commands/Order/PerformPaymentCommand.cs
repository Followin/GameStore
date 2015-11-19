using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;
using GameStore.Static;

namespace GameStore.BLL.Commands.Order
{
    public class PerformPaymentCommand : ICommand
    {
        public PaymentMethod Method { get; set; }

        public String Name { get; set; }

        public String Number { get; set; }

        public DateTime ExpirationDate { get; set; }

        public String Cvv2 { get; set; }

        public Decimal Sum { get; set; }
    }
}
