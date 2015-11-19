using System;
using System.Collections.Generic;

namespace GameStore.Domain.Entities
{
    public class PlatformType : Entity<Int32>
    {
        public String Name { get; set; }

        public virtual ICollection<Game> Games { get; set; } 
    }
}
