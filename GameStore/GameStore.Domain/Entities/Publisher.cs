using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.Entities
{
    public class Publisher : Entity<int>
    {
        public string CompanyName { get; set; }

        public string Description { get; set; }

        public string HomePage { get; set; }

        [NotMapped]
        public ICollection<Game> Games { get; set; } 
    }
}
