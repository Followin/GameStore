using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankService.Abstract
{
    public interface IMessageService
    {
        Task SendSms(String number, String message);

        Task SendEmail(String email, String message);
    }
}
