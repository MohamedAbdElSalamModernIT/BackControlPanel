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
    public class DrwaingPluginDto : IRegister
    {
        public string Id { get; set; }
        public FileType FileType { get; set; }
        public DrawingType DrawingType { get; set; }
        public string FileLink { get; set; }
        public DrawingStatus Status { get; set; }
        public string CustomerName { get; set; }
        public string OfficeName { get; set; }
        public DrwaingBaladia Baladia { get; set; }
        public DrwaingBaladia Amana { get; set; }
        public DrwaingBaladia BuildingType { get; set; }
        public int RequestNo { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Drawing, DrwaingPluginDto>()
                .Map(dest => dest.OfficeName, src => src.Client != null ? src.Client.OfficeName : "")
                .Map(dest => dest.Amana, src => src.Baladia.Amana);
        }
    }


    public class DrwaingBaladia
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
