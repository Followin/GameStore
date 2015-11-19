using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.Entities
{
    public class Publisher : Entity<Int32>
    {
        public String CompanyName { get; set; }

        public String Description { get; set; }

        public String HomePage { get; set; }

        [NotMapped]
        public ICollection<Game> Games { get; set; } 
    }
}
