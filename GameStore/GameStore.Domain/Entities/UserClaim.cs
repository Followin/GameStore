using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.Entities
{
    public class UserClaim
    {
        public Int32 Id { get; set; }

        public Int32 UserId { get; set; }

        public virtual User User { get; set; }

        public String Type { get; set; }

        public String Value { get; set; }

        public String Issuer { get; set; }
    }
}
