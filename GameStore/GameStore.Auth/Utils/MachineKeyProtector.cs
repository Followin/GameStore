using System;
using System.Web.Security;
using Microsoft.Owin.Security.DataProtection;

namespace GameStore.Auth.Utils
{
    public class MachineKeyProtector : IDataProtector
    {
        private String _purpose = "auth";

        public byte[] Protect(byte[] userData)
        {
            return MachineKey.Protect(userData, _purpose);
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return MachineKey.Unprotect(protectedData, _purpose);
        }
    }
}