using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands.Game
{
    public class AddGameVisitCommand : ICommand
    {
        public int GameId { get; set; }

        public int UserId { get; set; }
    }
}
