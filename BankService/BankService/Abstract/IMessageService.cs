using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankService.Abstract
{
    public interface IMessageService
    {
        void SendSms(String number, String message);

        void SendEmail(String email, String message);
    }
}
