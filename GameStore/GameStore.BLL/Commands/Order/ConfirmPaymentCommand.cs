using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands.Order
{
    public class ConfirmPaymentCommand : ICommand
    {
        public string Code { get; set; }
    }
}
