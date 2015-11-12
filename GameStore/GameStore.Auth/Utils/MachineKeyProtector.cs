using System;
using System.Web.Security;
using Microsoft.Owin.Security.DataProtection;

namespace GameStore.Auth.Utils
{
    public class MachineKeyProtector : IDataProtector
    {
        private readonly String _purpose;

        public MachineKeyProtector(string purpose)
        {
            _purpose = purpose;
        }

        public byte[] Protect(byte[] userData)
        {
            return MachineKey.Protect(userData, _purpose);
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            try
            {
                return MachineKey.Unprotect(protectedData, _purpose);
            }
            catch
            {
                return null;
            }
        }
    }
}