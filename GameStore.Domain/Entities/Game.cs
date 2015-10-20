using System;
using System.Collections.Generic;
using GameStore.Domain.Abstract;

namespace GameStore.Domain.Entities
{
    public class Game : Entity<Int32>
    {
        public String Key { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<PlatformType> PlatformTypes { get; set; } 
    }
}
