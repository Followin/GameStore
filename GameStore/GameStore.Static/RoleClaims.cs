using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace GameStore.Static
{
    public static class RoleClaims
    {
        private static Dictionary<String, IEnumerable<Claim>> _roleClaims;

        static RoleClaims()
        {
            _roleClaims = new Dictionary<string, IEnumerable<Claim>>
            {
                {
                    Roles.Admin,
                    new[]
                    {
                        new Claim(ClaimTypesExtensions.UserPermission, Permissions.Full),
                    }
                },
                {
                    Roles.Manager,
                    new[]
                    {
                        new Claim(ClaimTypesExtensions.Genre, Permissions.Full), 
                        new Claim(ClaimTypesExtensions.PublisherPermission, Permissions.Full),
                        new Claim(ClaimTypesExtensions.GamePermission, Permissions.Full),
                        new Claim(ClaimTypesExtensions.OrderPermission, Permissions.Edit),
                    }
                },
                {
                    Roles.Moderator,
                    new[]
                    {
                        new Claim(ClaimTypesExtensions.UserPermission, Permissions.Ban),
                        new Claim(ClaimTypesExtensions.CommentPermission, Permissions.Delete), 
                        new Claim(ClaimTypesExtensions.CommentPermission, Permissions.Add),  
                    }
                },
                {
                    Roles.User,
                    new[]
                    {
                        new Claim(ClaimTypesExtensions.CommentPermission, Permissions.Add), 
                    }
                },
                {
                    Roles.Guest,
                    new[]
                    {
                        new Claim(ClaimTypesExtensions.CommentPermission, Permissions.Add),
                    }
                }
            };
        }

        public static IEnumerable<Claim> GetClaimsForRole(String role)
        {
            if (_roleClaims.ContainsKey(role))
            {
                return _roleClaims[role];
            }
            return Enumerable.Empty<Claim>();
        }
    }
}
