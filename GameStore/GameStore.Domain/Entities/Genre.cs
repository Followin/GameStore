using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Domain.Entities
{
    public class Genre : Entity<Int32>
    {
        public String NameRu { get; set; }

        public String NameEn { get; set; }

        public Int32? ParentGenreId { get; set; }

        public virtual Genre ParentGenre { get; set; }

        public virtual ICollection<Genre> ChildGenres { get; set; } 

        [NotMapped]
        public virtual ICollection<Game> Games { get; set; }
    }
}
