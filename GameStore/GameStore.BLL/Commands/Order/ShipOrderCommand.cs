using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands.Order
{
    public class ShipOrderCommand : ICommand
    {
        public int Id { get; set; }
    }
}
