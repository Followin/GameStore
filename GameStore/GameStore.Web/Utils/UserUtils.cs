using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace GameStore.Web.Utils
{
    public static class UserUtils
    {
        public static string GetRoles()
        {
            if (HttpContext.Current.User != null && (HttpContext.Current.User as ClaimsPrincipal) != null)
            {
                return ((ClaimsPrincipal)HttpContext.Current.User).FindAll(x => x.Type == ClaimTypes.Role)
                                                            .Aggregate("", (res, role) =>
                                                                res + role.Value);
            }
            return "";
        }
    }
}