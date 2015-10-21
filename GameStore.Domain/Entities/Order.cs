using System;
using System.Collections.Generic;
using GameStore.Domain.Abstract;

namespace GameStore.Domain.Entities
{
    public class Order : Entity<Int32>
    {
        public Int32 CustomerId { get; set; }

        public DateTime Time { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; } 
    }
}
