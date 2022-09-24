using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Auth;
using Domain.Enums.Roles;

namespace Infrastructure.Interfaces {
  public interface IPermissionService {
    List<ModulePermission> GetPermissions();
    Task AddPermissionsToRole(Role role, List<PermissionKeys> permission);
    Task AddPermissionsToRole(Role role, string[] permission);
    List<string> GetPermissionListForUser(User userId);
     
    }
}
