using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Common.Attributes;
using Common.Extensions;
using Domain.Entities;
using Domain.Entities.Auth;
using Domain.Enums.Roles;
using Domain.Extensions;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public class PermissionService : IPermissionService
    {
        private readonly IServiceProvider _scope;
        private readonly IAppDbContext _context;

        public PermissionService(IServiceProvider scope, IAppDbContext context)
        {
            _scope = scope;
            _context = context;
        }

        public List<ModulePermission> GetPermissions()
        {
            var modulePermissions = new List<ModulePermission>();
            foreach (SystemModule module in Enum.GetValues(typeof(SystemModule)))
            {
                var list = GetModulePermissions(module)
                  .GroupBy(p => p.GetAttribute<DescribePermissionAttribute>().Group)
                  .Select(s => new GroupedPermission()
                  {
                      Name = s.Key,
                      Permissions = s.Select(p => p.GetAttribute<DescribePermissionAttribute>().Title).ToList()
                  }
                  ).ToList();

                modulePermissions.Add(new ModulePermission()
                {
                    Name = module.GetAttribute<EnumMemberAttribute>().Value,
                    Permissions = list
                });
            }
            return modulePermissions;
        }

        private IEnumerable<PermissionKeys> GetModulePermissions(SystemModule module)
        {
            foreach (PermissionKeys item in Enum.GetValues(typeof(PermissionKeys)))
            {
                if (item.GetAttribute<DescribePermissionAttribute>().Module == module)
                    yield return item;
            }
        }

        public async Task AddPermissionsToRole(Role role, List<PermissionKeys> permissions)
        {
            foreach (var permission in permissions)
            {
                var userManager = _scope.GetService<UserManager<User>>();

                role.Permissions ??= "";
                var moduleAttribute = permission.GetAttribute<DescribePermissionAttribute>();
                if (!role.Permissions.Contains((char)permission))
                {
                    role.Permissions += (char)permission;
                    if (moduleAttribute != null)
                    {
                        SystemModule module = moduleAttribute.Module;
                        var users = await userManager.GetUsersInRoleAsync(role.Name);
                        foreach (var user in users)
                        {
                            if (((user.AllowedModules & module) == 0))
                            {
                                //add user to this module
                                user.AllowedModules |= module;
                            }

                            await userManager.UpdateAsync(user);
                        }
                    }
                }
            }
        }

        public async Task AddPermissionsToRole(Role role, string[] permissions)
        {
            await AddPermissionsToRole(role, ConvertStingToPermisson(permissions));
        }

        private List<PermissionKeys> ConvertStingToPermisson(string[] strs)
        {
            var attrs = (typeof(PermissionKeys)).GetFields().ToList();

            List<PermissionKeys> permissionsList = new List<PermissionKeys>();
            foreach (var str in strs)
            {
                foreach (var attr in attrs)
                {
                    if (attr.GetCustomAttribute<DescribePermissionAttribute>() != null)
                    {
                        if (str == attr.GetCustomAttribute<DescribePermissionAttribute>().Title)
                            permissionsList.Add((PermissionKeys)attr.GetValue((typeof(PermissionKeys))));
                    }
                }
            }


            return permissionsList;
        }

        public List<string> GetPermissionListForUser(User appUser)
        {
            var permissionsList = appUser.UserRoles
            .Select(ur => ur.Role.Permissions.GetPermissionsFromString().getPermissionTitle());


            List<string> permissions = new List<string>();

            foreach (var permission in permissionsList)
            {
                permission.ToList().ForEach(p =>
                {
                    permissions.Add(p);
                });
            }


            return permissions;



        }
    }


}