using System;
using GameStore.BLL.Abstract;

namespace GameStore.BLL.Observer
{
    public class EmailManager : IObserver<string>
    {
        private IMessageSender _sender;
        private string email;


        public EmailManager(IMessageSender sender, string email)
        {
            _sender = sender;
            this.email = email;
        }

        public async void OnNext(string value)
        {
            await _sender.SendSmsAsync(email, value);
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
