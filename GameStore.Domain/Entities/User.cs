using System;
using System.Collections.Generic;
using GameStore.Domain.Abstract;

namespace GameStore.Domain.Entities
{
    public class User : Entity<Int32>
    {
        public String SessionId { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Game> ViewedGames { get; set; } 
    }
}
