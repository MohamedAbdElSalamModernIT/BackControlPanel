using Application.Baladya.Dto;
using Common.Extensions;
using Domain.Entities.Benaa;
using Domain.Enums;
using Mapster;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public DrwaingBaladia Baladia { get; set; }
        public DrwaingBaladia Amana { get; set; }
        public DrwaingBaladia BuildingType { get; set; }
        public int RequestNo { get; set; }
        public string EngineerId { get; set; }
        public OfficeDrawingStatus OfficeStatus { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string Comments { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Drawing, DrwaingPluginDto>()
                .Map(dest => dest.Amana, src => src.Baladia.Amana);
        }
    }


    public class DrwaingBaladia : IRegister
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<BuildingType, DrwaingBaladia>()
                .Map(dest => dest.ID, src => src.ExternalCode.Value);
        }
    }
}
