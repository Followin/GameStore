using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands.User
{
    public class ChangeNotificationTypeCommand : ICommand
    {
        public int UserId { get; set; }

        public string PreferenceType { get; set; }
    }
}
