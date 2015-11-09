using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands.User
{
    public class CreateUserCommand : ICommand
    {
        public String SessionId { get; set; }
    }
}
