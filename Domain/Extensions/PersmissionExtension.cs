using System.Collections.Generic;
using System.Linq;
using Common.Attributes;
using Common.Extensions;
using Domain.Enums.Roles;

namespace Domain.Extensions
{
    public static class PersmissionExtension
    {
        public static IEnumerable<string> getPermissionTitle(this IEnumerable<PermissionKeys> permissions)
        {
            return permissions.Select(s => s.GetAttribute<DescribePermissionAttribute>().Title).ToArray(); 
        }
        public static IEnumerable<PermissionKeys> GetPermissionsFromString(this string permisssions)
        {
            if (!string.IsNullOrEmpty(permisssions))
            {
                foreach (var chr in permisssions)
                {
                    yield return (PermissionKeys)chr;
                }
            }

        }
    }
}