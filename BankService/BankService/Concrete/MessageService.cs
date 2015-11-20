using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BankService.Abstract;
using Twilio;

namespace BankService.Concrete
{
    public class MessageService : IMessageService
    {
        public Task SendSms(string number, string message)
        {
            string AccountSid = "AC31ae3bf014300c048db3e592b8a15651";

            string AuthToken = "e4cd7422e0e7b9d0837377781180d95e";

            string twilioPhoneNumber = "+12515453790";

            var twilio = new TwilioRestClient(AccountSid, AuthToken);
            twilio.SendMessage(twilioPhoneNumber, number, message);

            return Task.FromResult(0);
        }

        public Task SendEmail(string email, string message)
        {
            return Task.FromResult(0);
        }
    }
}