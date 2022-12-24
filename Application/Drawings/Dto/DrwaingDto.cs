using Application.Baladya.Dto;
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
        public string OfficeName { get; set; }
        public DrwaingBaladia Baladia { get; set; }
        public DrwaingBuildingType BuildingType { get; set; }
        public int RequestNo { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Drawing, DrwaingDto>()
                .Map(dest => dest.Baladia, src => src.Baladia)
                .Map(dest => dest.OfficeName, src => src.Client != null ? src.Client.OfficeName : "")
                .Map(dest => dest.BuildingType, src => src.BuildingType);
        }
    }


    public class DrwaingBaladia
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class DrwaingBuildingType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
