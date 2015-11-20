using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Domain.Entities
{
    public class Genre : Entity<int>
    {
        public string NameRu { get; set; }

        public string NameEn { get; set; }

        public int? ParentGenreId { get; set; }

        public virtual Genre ParentGenre { get; set; }

        public virtual ICollection<Genre> ChildGenres { get; set; } 

        [NotMapped]
        public virtual ICollection<Game> Games { get; set; }
    }
}
