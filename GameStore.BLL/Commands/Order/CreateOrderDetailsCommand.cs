using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands.Order
{
    public class CreateOrderDetailsCommand : ICommand
    {
        public Decimal Price { get; set; }

        public float Discount { get; set; }

        public Int16 Quantity { get; set; }

        public Int32 GameId { get; set; }

        public Int32 OrderId { get; set; }
    }
}
