using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Domain.Entities
{
    public class User : Entity<Int32>
    {
        public String Name { get; set; }

        public String PasswordHash { get; set; }

        public String SecurityStamp { get; set; }

        public DateTime? BanExpirationTime { get;set; }

        public virtual ICollection<UserClaim> Claims { get; set; } 
    }
}
