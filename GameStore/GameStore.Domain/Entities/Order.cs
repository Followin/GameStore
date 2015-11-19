using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Domain.Entities
{
    public class Order : Entity<Int32>
    {
        public Int32 UserId { get; set; }

        //public User User { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; } 
    }
}
