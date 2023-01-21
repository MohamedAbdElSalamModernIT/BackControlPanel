using System;
using System.Linq;
using AutoMapper;
using Common.Interfaces.Mapper;
using Domain.Entities;
using Domain.Entities.Auth;
using Domain.Enums;

namespace Application.UserManagment.Dto
{
    public class UserDto : IHaveCustomMapping
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string[] Roles { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int UserType { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public int? AmanaId { get; set; }
        public int? BaladiaId { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<AppUser, UserDto>()
              .ForMember(a => a.FullName, cfg => cfg.MapFrom(a => a.FullName))
              .ForMember(a => a.Roles, cfg => cfg.MapFrom(a => a.UserRoles.Count > 0 ? a.UserRoles.Select(s => s.Role.Name).ToArray() : default))

              ;


        }
    }

    public class ClientDto
    {
        public string IdentityId { get; set; }
        public int? BaladiaId { get; set; }
        public int? AmanaId { get; set; }
        public string OfficeId { get; set; }

    }
}