using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands.Order
{
    public class ShipOrderCommand : ICommand
    {
        public Int32 Id { get; set; }
    }
}
