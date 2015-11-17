using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands.Order
{
    public class DeleteOrderDetailsCommand : ICommand
    {
        public Int32 GameId { get; set; }

        public Int32 OrderId { get; set; }
    }
}
