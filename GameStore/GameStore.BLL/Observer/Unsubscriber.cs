using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Observer
{
    public class Unsubscriber : IDisposable
    {
        private IList<IObserver<string>> _observers;
        private IObserver<string> _observer;

        public int Id { get; private set; }

        public Unsubscriber(IList<IObserver<string>> observers, IObserver<string> observer, int id)
        {
            _observers = observers;
            _observer = observer;
            Id = id;
        }

        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
            {
                _observers.Remove(_observer);
            }
        }
    }
}
