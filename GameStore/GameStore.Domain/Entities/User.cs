using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.Domain.Abstract;

namespace GameStore.Domain.Entities
{
    public class User : Entity<Int32>
    {
        public String SessionId { get; set; }

        //public virtual ICollection<Order> Orders { get; set; }

        [NotMapped]
        public virtual ICollection<Game> ViewedGames { get; set; } 
    }
}
