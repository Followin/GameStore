using System;
using GameStore.Domain.Abstract;

namespace GameStore.Domain.Entities
{
    public class OrderDetails : Entity<Int32>
    {
        public Double Price { get; set; }

        public float Discount { get; set; }
 
        public Int16 Quantity { get; set; }

        public Int32 GameId { get; set; }

        public virtual Game Game { get; set; }

        public Int32 OrderId { get; set; }

        public Order Order { get; set; }
    }
}
