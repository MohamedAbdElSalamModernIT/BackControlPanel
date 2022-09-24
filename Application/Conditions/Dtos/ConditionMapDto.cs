using Common;
using Domain.Entities.Benaa;
using Infrastructure.Interfaces;
using Mapster;
using System.Collections.Generic;


namespace Application.Conditions.Dtos
{

    public class ConditionsMapDto : BaseEntityAudit ,IRegister
    {
        public int AlBaladiaID { get; set; }
        public int BuildingTypeID { get; set; }
        public string BuildingType { get; set; }
        public string Condition { get; set; }
        public string Category { get; set; }
        public string ParentCategory { get; set; }
        public string Baldia { get; set; }
        public string Amana { get; set; }
        public double ConditionID { get; set; }
        public string VersionId { get; set; }

        public List<Parameter> Parameters { get; set; }



        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ConditionsMap, ConditionsMapDto>()
                 .Map(dest => dest.BuildingType, src => src.BuildingType.Name)
                 .Map(dest => dest.Baldia, src => src.Baladia.Name)
                 .Map(dest => dest.Condition, src => src.Condition.Description)
                 .Map(dest => dest.Amana, src => src.Baladia.Amana.Name)
                 .Map(dest => dest.Category, src => src.Condition.Category.Name)
                 .Map(dest => dest.VersionId, src => src.Condition.Version.VersionId)
                 .Map(dest => dest.ParentCategory, src => src.Condition.Category
                 .ParentCategory.Name)
                 ;
        }
    }

}
