using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands.Game
{
    public class DeleteGameCommand : ICommand
    {
        public String Key { get; set; }
    }
}
