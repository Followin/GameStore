using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Domain.Entities
{
    public class Game : Entity<int>
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string DescriptionRu { get; set; }

        public string DescriptionEn { get; set; }

        public decimal Price { get; set; }

        public short UnitsInStock { get; set; }

        public bool Discontinued { get; set; }

        public DateTime PublicationDate { get; set; }

        public DateTime IncomeDate { get; set; }

        public int? PublisherId { get; set; }

        [NotMapped]
        public virtual Publisher Publisher { get; set; }

        [NotMapped]
        public virtual ICollection<Comment> Comments { get; set; } 

        [NotMapped]
        public virtual ICollection<Genre> Genres { get; set; }

        public virtual ICollection<PlatformType> PlatformTypes { get; set; }

        public virtual ICollection<User> UsersViewed { get; set; } 
    }
}
