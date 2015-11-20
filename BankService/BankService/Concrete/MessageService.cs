using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
            SmtpClient client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587
            };

            System.Net.NetworkCredential credentials =
            new System.Net.NetworkCredential("aspromeo@gmail.com", "admin1488");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            MailMessage msg = new MailMessage { From = new MailAddress("aspromeo@gmail.com") };

            msg.To.Add(new MailAddress(email));
            msg.Subject = "Purchase confirmation";
            msg.IsBodyHtml = true;
            msg.Body = string.Format("<html><head></head>" +
                "<body>" +
                "<section><h2>GameStore</h2><p>" + message + "</p></section></body></html>");

            return client.SendMailAsync(msg);
        }
    }
}