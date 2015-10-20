using System;
using System.Collections.Generic;
using GameStore.Domain.Abstract;

namespace GameStore.Domain.Entities
{
    public class Genre : Entity<Int32>
    {
        public String Name { get; set; }
        public Int32? ParentGenreId { get; set; }
        public virtual Genre ParentGenre { get; set; }
        public virtual ICollection<Genre> ChildGenres { get; set; } 
        public virtual ICollection<Game> Games { get; set; }
        
    }
}
