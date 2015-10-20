using System;
using System.Collections.Generic;

namespace GameStore.Domain.Abstract.Entities
{
    public abstract class GameBase : Entity<Int32>
    {
        public abstract String Key { get; set; }

        public abstract String Name { get; set; }

        public abstract String Description { get; set; }

        public abstract ICollection<GenreBase> Genres { get; set; }
    }
}
