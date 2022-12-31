using System;
using System.Collections.Generic;
using Common;
using Common.Attributes;
using Common.Interfaces;
using Domain.Entities.Notification;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Auth
{
    //[Table("AspNetUsers")]
    public class User : IdentityUser, IAudit, IDeleteEntity, IBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public SystemModule AllowedModules { get; set; }
        public string Permissions { get; set; }
        public bool Active { get; set; } = true;
        public UserType UserType { get; set; }
        public HashSet<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    }

}