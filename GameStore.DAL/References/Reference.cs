using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.References
{
    public class Reference
    {
        public Int32 Id { get; set; }

        public DatabaseNames DatabaseName { get; set; }

        public Int32 MatchesId { get; set; }

        public Boolean IsDeleted { get; set; }

    }
}
