using System;
using System.Collections.Generic;

namespace GameStore.Domain.Abstract.Entities
{
    public abstract class GenreBase : Entity<Int32>
    {
        public abstract String Name { get; set; }
        public abstract ICollection<GameBase> Games { get; set; } 
    }
}
