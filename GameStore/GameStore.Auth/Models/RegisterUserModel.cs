using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace GameStore.Auth.Models
{
    public class RegisterUserModel
    {
        public String Name { get; set; }

        public String Password { get; set; }

        public IEnumerable<Claim> Claims { get; set; }
    }
}
