using System;
using System.Security.Principal;

namespace GameStore.Web.Abstract
{
    public interface ICustomPrincipal : IPrincipal
    {
        int Id { get; set; }

        string SessionId { get; set; }
    }
}
