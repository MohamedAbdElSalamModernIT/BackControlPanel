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
    public class DrwaingDetailsDto : IRegister
    {
        public string Id { get; set; }
        public string FileType { get; set; }
        public string DrawingType { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public string FileLink { get; set; }
        public string OfficeName { get; set; }
        public string EngineerId { get; set; }
        public string EngineerName { get; set; }
        public string Baladia { get; set; }
        public string BuildingType { get; set; }
        public int RequestNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool HasFile { get; set; }

        public string OfficeStatus { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string Comments { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Drawing, DrwaingDetailsDto>()
                .Map(dest => dest.Baladia, src => src.Baladia != null ? src.Baladia.Name : "")
                .Map(dest => dest.BuildingType, src => src.BuildingType != null ? src.BuildingType.Name : "")
                .Map(dest => dest.OfficeName, src => src.Office.Name)
                .Map(dest => dest.EngineerName, src => src.Engineer.FullName)
                .Map(dest => dest.DrawingType, src => src.DrawingType.GetAttribute<DescriptionAttribute>().Description)
                .Map(dest => dest.Status, src => src.Status.GetAttribute<DescriptionAttribute>().Description)
                .Map(dest => dest.FileType, src => src.FileType.GetAttribute<DescriptionAttribute>().Description)
                .Map(dest => dest.OfficeStatus, src => src.OfficeStatus.GetAttribute<DescriptionAttribute>().Description)
                .Map(dest => dest.HasFile, src => src.File != null)
            ;
        }
    }


    public class DrwaingDto 
    {
        public string Id { get; set; }
        public int FileType { get; set; }
        public int DrawingType { get; set; }
        public string CustomerName { get; set; }
        public int Status { get; set; }
        public string FileLink { get; set; }
        public string EngineerId { get; set; }
        public int BaladiaId { get; set; }
        public int BuildingTypeId { get; set; }
        public int RequestNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int OfficeStatus { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string Comments { get; set; }

    }

}
