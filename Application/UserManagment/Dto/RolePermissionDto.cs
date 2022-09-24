using AutoMapper;
using Common.Interfaces.Mapper;
using Domain.Entities.Auth;
using Domain.Extensions;

namespace Application.UserManagment.Dto
{
    public  class RolePermissionDto :IHaveCustomMapping {
        public string Id { get; set; }
        public string Name { get; set; }
        public string[] Permissions { get; set; }

        public void CreateMappings(Profile configuration) {
            configuration.CreateMap<Role, RolePermissionDto>()
                          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                          .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                          .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions.GetPermissionsFromString().getPermissionTitle()));
        }

    }
}
