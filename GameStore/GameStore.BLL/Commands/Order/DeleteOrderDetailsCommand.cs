using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands.Order
{
    public class DeleteOrderDetailsCommand : ICommand
    {
        public int GameId { get; set; }

        public int OrderId { get; set; }
    }
}
