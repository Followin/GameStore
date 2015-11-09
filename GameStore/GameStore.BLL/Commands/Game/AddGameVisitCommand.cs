using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands.Game
{
    public class AddGameVisitCommand : ICommand
    {
        public Int32 GameId { get; set; }

        public Int32 UserId { get; set; }
    }
}
