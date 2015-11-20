using System;
using System.Collections.Generic;

namespace GameStore.Domain.Entities
{
    public class PlatformType : Entity<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Game> Games { get; set; } 
    }
}
