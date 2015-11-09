using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.Domain.Abstract;

namespace GameStore.Domain.Entities
{
    public class Game : Entity<Int32>
    {
        public String Key { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public Decimal Price { get; set; }

        public Int16 UnitsInStock { get; set; }

        public Boolean Discontinued { get; set; }

        public DateTime PublicationDate { get; set; }

        public DateTime IncomeDate { get; set; }

        public Int32 PublisherId { get; set; }

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
