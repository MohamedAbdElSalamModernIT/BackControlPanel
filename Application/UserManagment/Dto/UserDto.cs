using System.Linq;
using AutoMapper;
using Common.Interfaces.Mapper;
using Domain.Entities;
using Domain.Entities.Auth;

namespace Application.UserManagment.Dto {
  public class UserDto:IHaveCustomMapping {
    public string Id { get; set; }
    public string  FullName { get; set; }
    public string UserName { get; set; }
    public string[] Roles { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool Active { get; set; }

    public void CreateMappings(Profile configuration) {
      configuration.CreateMap<AppUser, UserDto>()
        .ForMember(a => a.FullName, cfg => cfg.MapFrom(a =>a.FullName))
        .ForMember(a=>a.Roles,cfg=>cfg.MapFrom(a=>a.UserRoles.Count>0 ?a.UserRoles.Select(s=>s.Role.Name).ToArray():default))

        ;


    }
  }
}