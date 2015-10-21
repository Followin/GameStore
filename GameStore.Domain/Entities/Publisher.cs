using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Domain.Abstract;

namespace GameStore.Domain.Entities
{
    public class Publisher : Entity<Int32>
    {
        public String ComanyName { get; set; }

        public String Description { get; set; }

        public String HomePage { get; set; }

        public ICollection<Game> Games { get; set; } 
    }
}
