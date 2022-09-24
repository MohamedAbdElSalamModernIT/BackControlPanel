using AutoMapper;
using Common.Interfaces.Mapper;
using Domain.Entities.Auth;

namespace Application.UserManagment.Dto {
    public class RoleDto:IHaveCustomMapping {
        public string Id { get; set; }
        public string Name { get; set; }
    
        public void CreateMappings(Profile configuration) {
            configuration.CreateMap<Role, RoleDto>()
                          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                          .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
                        
        }           
    }
}
