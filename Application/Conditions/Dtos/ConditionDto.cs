using Common.Infrastructures;
using System.Collections.Generic;
using Mapster;
using System;
using Infrastructure.Interfaces;
using Domain.Entities.Benaa;
using System.Linq;
using Common;

namespace Application.Conditions.Dtos
{
    public class ConditionDto : BaseEntityAudit, IRegister
    {
        public double ID { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string VersionId{ get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Condition, ConditionDto>()
                .Map(dest => dest.Category, src => src.Category.Name)
                .Map(dest => dest.Category, src => src.Version.VersionId);
        }
    }
    public class ConditionWithParameters :IRegister
    {
        public double ID { get; set; }
        public string Name { get; set; }
        public string ParametersValues { get; set; }


        public List<Parameter> Parameters { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ConditionsMap, ConditionWithParameters>()
                .Map(dest => dest.ID, src => src.ConditionID)
                .Map(dest => dest.Name, src => src.Condition.Description);
        }
    }
}
