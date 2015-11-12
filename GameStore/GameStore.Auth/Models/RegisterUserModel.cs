using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Auth.Models
{
    public class RegisterUserModel
    {
        public String Name { get; set; }

        public String Password { get; set; }

        public String Roles { get; set; }
    }
}
