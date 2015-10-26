using System;
using System.Security.Principal;

namespace GameStore.Web.Abstract
{
    public interface ICustomPrincipal : IPrincipal
    {
        Int32 Id { get; set; }
        String SessionId { get; set; }
    }
}
