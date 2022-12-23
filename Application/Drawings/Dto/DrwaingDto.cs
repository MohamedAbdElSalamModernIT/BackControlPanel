using Domain.Entities.Benaa;
using Domain.Enums;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Drawings.Dto
{
    public class DrwaingDto : IRegister
    {
        public string Id { get; set; }
        public FileType FileType { get; set; }
        public DrawingType DrawingType { get; set; }
        public string FileLink { get; set; }
        public DrawingStatus Status { get; set; }
        public string CustomerName { get; set; }

        public string Baladia { get; set; }
        public string BuildingType { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Drawing, DrwaingDto>()
                .Map(dest => dest.Baladia, src => src.Baladia != null ? src.Baladia.Name : "")
                .Map(dest => dest.BuildingType, src => src.BuildingType != null ? src.BuildingType.Name : "");
        }
    }
}
