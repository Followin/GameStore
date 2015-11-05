﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.Domain.Abstract;

namespace GameStore.Domain.Entities
{
    public class Order : Entity<Int32>
    {
        public Int32 UserId { get; set; }

        //public User User { get; set; }

        public DateTime? Time { get; set; }

        public Boolean Payed { get;set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; } 
    }
}
