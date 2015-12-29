using GameStore.BLL.CQRS;
using GameStore.Static;

namespace GameStore.BLL.Commands.User
{
    public class ChangeNotificationTypeCommand : ICommand
    {
        public int UserId { get; set; }

        public string PreferenceType { get; set; }
    }
}
