using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankService.Models
{
    public enum PaymentResult
    {
        Success,
        Fail,
        NotEnoughMoney,
        CodeConfirmRequired,
        CardDoesntExist
    }
}