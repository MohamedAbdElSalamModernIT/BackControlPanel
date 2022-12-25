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
    public class ConditionResultDto :IRegister
    {
        public string Condition { get; set; }
        public string Status { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ConditionResult, ConditionResultDto>()
                .Map(dest => dest.Status, src => src.Status.ToString())
                .Map(dest => dest.Condition, src => src.CurrentCondition);
        }
    }
}
