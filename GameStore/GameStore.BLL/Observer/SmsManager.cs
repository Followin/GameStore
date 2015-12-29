using System;
using GameStore.BLL.Abstract;

namespace GameStore.BLL.Observer
{
    public class SmsManager : IObserver<string>
    {
        private IMessageSender _messageSender;
        private string phone;

        public SmsManager(IMessageSender messageSender, string phone)
        {
            _messageSender = messageSender;
            this.phone = phone;
        }

        public void OnNext(string value)
        {
            _messageSender.SendSmsAsync(phone, value);
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
    }
}
