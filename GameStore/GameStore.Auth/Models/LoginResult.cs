using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Auth.Models
{
    public enum LoginResultStatus
    {
        Success,
        WrongCredentials
    }

    public class LoginResult
    {
        public LoginResultStatus Status { get; set; }
    }


}


