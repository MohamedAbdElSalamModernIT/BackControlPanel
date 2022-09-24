using System.Collections.Generic;
using Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Auth {
  public class Role : IdentityRole ,IBaseEntity{
    public Role(string name) : base(name) {
      Name = name;
    }
    public HashSet<UserRole> UserRoles { get; set; }
    public string Permissions { get; set; }
  }

}
