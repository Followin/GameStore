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

        public string Name { get; set; }

        public string Number { get; set; }

        public int ExpirationMonth { get; set; }

        public int ExpirationYear { get; set; }

        public string Cvv2 { get; set; }

        public decimal Sum { get; set; }
    }
}
