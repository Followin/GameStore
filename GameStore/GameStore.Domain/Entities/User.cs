using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Domain.Entities
{
    public class User : Entity<int>
    {
        public string Name { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public DateTime? BanExpirationTime { get;set; }

        public virtual ICollection<UserClaim> Claims { get; set; } 
    }
}
