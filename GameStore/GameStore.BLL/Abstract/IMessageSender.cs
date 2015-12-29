using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Abstract
{
    public interface IMessageSender
    {
        Task SendEmailAsync(string email, string message);

        Task SendSmsAsync(string phone, string message);
    }
}
