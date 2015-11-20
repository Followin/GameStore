using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands.Order
{
    public class CreateOrderDetailsCommand : ICommand
    {
        public decimal Price { get; set; }

        public float Discount { get; set; }

        public short Quantity { get; set; }

        public int GameId { get; set; }

        public int OrderId { get; set; }
    }
}
