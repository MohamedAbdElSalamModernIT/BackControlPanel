using Application.UserManagment.Dto;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Office.Dtos
{
    public class OfficeDto : IRegister
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public string Amana { get; set; }
        public string Owner { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Benaa.Office, OfficeDto>()
                .Map(dest => dest.Owner, src => src.Owner != null ? src.Owner.FullName : "")
                .Map(dest => dest.Amana, src => src.Amana != null ? src.Amana.Name : "");

        }
    }
    
    public class OfficeDetailsDto 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AmanaId { get; set; }
        public UserDto Owner { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        
    }
}
