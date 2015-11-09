using System;
using System.Collections.Generic;
using GameStore.Domain.Abstract;

namespace GameStore.Domain.Entities
{
    public class PlatformType : Entity<Int32>
    {
        public String Name { get; set; }

        public virtual ICollection<Game> Games { get; set; } 
    }
}
