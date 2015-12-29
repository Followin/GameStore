using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.Abstract;
using GameStore.DAL.Abstract;
using GameStore.Domain.Entities;
using GameStore.Static;

namespace GameStore.BLL.Observer
{
    public class OrderNotificationSiren : IObservable<string>
    {
        public IList<Unsubscriber> Unsubscribers { get; private set; }

        private IList<IObserver<string>> _observers;
        private IMessageSender _sender;
        private IGameStoreUnitOfWork _db;

        public OrderNotificationSiren(IMessageSender sender, IGameStoreUnitOfWork db)
        {
            _observers = new List<IObserver<string>>();
            Unsubscribers = new List<Unsubscriber>();

            _sender = sender;
            _db = db;

            var usersWithPreferences =
                _db.Users.Get(x => x.Claims.Any(y => y.Type == ClaimTypesExtensions.NotificationPreferenceType));
            foreach (var usersWithPreference in usersWithPreferences)
            {
                SubscribeUser(usersWithPreference);
            }
        }

        public IDisposable Subscribe(IObserver<string> observer, int id)
        {
            _observers.Add(observer);

            var unsubscriber = new Unsubscriber(_observers, observer, id);
            Unsubscribers.Add(unsubscriber);

            return unsubscriber;
        } 

        public IDisposable Subscribe(IObserver<string> observer)
        {
            _observers.Add(observer);

            return null;
        }

        public IDisposable SubscribeUser(User user)
        {
            var userPreference =
                user.Claims.FirstOrDefault(x => x.Type == ClaimTypesExtensions.NotificationPreferenceType).Value;
            IObserver<string> observer = null;

            switch (userPreference)
            {
                case NotificationPreferenceTypes.Email:
                    observer = new EmailManager(_sender, user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value);
                    break;
                case NotificationPreferenceTypes.Sms:
                    observer = new SmsManager(_sender, user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.MobilePhone).Value);
                    break;
            }

            _observers.Add(observer);

            var unsubscriber = new Unsubscriber(_observers, observer, user.Id);
            Unsubscribers.Add(unsubscriber);

            return unsubscriber;
        }


        public void Notify(string message)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(message);
            }
        }
    }
}
