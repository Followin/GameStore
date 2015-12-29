using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.Commands.User;
using GameStore.BLL.CQRS;
using GameStore.BLL.Observer;
using GameStore.DAL.Abstract;
using NLog;

namespace GameStore.BLL.CommandHandlers.User
{
    public class ChangeNotificationTypeCommandHandler : 
        ICommandHandler<ChangeNotificationTypeCommand>
    {
        private OrderNotificationSiren _orderNotificationSiren;
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public ChangeNotificationTypeCommandHandler(OrderNotificationSiren orderNotificationSiren, IGameStoreUnitOfWork db, ILogger logger)
        {
            _orderNotificationSiren = orderNotificationSiren;
            _db = db;
            _logger = logger;
        }
        
        public CommandResult Execute(ChangeNotificationTypeCommand command)
        {
            var user = _db.Users.Get(command.UserId);
            if (user == null)
            {
                throw new ArgumentOutOfRangeException("command", "User not found");
            }

            if (string.IsNullOrWhiteSpace(command.PreferenceType))
            {
                var unsubscriber = _orderNotificationSiren.Unsubscribers.FirstOrDefault(x => x.Id == user.Id);
                if (unsubscriber != null)
                {
                    unsubscriber.Dispose();
                }
            }
            else
            {
                _orderNotificationSiren.SubscribeUser(user);
            }

            return new CommandResult();
        }
    }
}
