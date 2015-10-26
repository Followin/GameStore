using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using GameStore.Web.Abstract;

namespace GameStore.Web.Concrete
{
    public class CustomPrincipal : ICustomPrincipal
    {
        public CustomPrincipal(String id)
        {
            Identity = new GenericIdentity(id);
        }

        public IIdentity Identity { get; private set; }

        public int Id { get; set; }

        public string SessionId { get; set; }

        public bool IsInRole(string role)
        {
            return false;
        }

        
    }
}