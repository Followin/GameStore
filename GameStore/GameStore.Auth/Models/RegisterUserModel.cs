using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace GameStore.Auth.Models
{
    public class RegisterUserModel
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public IEnumerable<Claim> Claims { get; set; }
    }
}
