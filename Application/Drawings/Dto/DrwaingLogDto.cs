using Common.Extensions;
using Domain.Entities.Benaa;
using Domain.Enums;
using Mapster;
using System;
using System.ComponentModel;

namespace Application.Drawings.Dto
{
    public class DrwaingLogDto: IRegister
    {
        public string Id { get; set; }
        public int SuccessNo { get; set; }
        public int FailNo { get; set; }
        public int OtherNo { get; set; }
        public string Result { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<DrawingLog, DrwaingLogDto>()
                .Map(dest => dest.Result, src => src.Result.GetAttribute<DescriptionAttribute>().Description);
        }
    }
}
