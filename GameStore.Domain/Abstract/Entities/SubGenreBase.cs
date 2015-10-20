using System;

namespace GameStore.Domain.Abstract.Entities
{
    public abstract class SubGenreBase : Entity<Int32>
    {
        public abstract String Name { get; set; }

        public abstract Int32 ParentGenreId { get; set; }

        public abstract GenreBase ParentGenre { get; set; }
    }
}
