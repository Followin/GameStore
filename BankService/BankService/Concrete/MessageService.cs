using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankService.Abstract;

namespace BankService.Concrete
{
    public class MessageService : IMessageService
    {
        public void SendSms(string number, string message)
        {
            //stub
        }

        public void SendEmail(string email, string message)
        {
            //stub
        }
    }
}