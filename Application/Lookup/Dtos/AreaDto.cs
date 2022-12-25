using Common.Infrastructures;
using System.Collections.Generic;
using Mapster;
using Domain.Entities.Benaa;
using Common;

namespace Application.Lookup.Dtos
{
    public class AmanaDto : IRegister
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Benaa.Amana, AmanaDto>().
                Map(dest => dest.Area, src => src.Area.Name);

        }
    }

    public class InformationDto :BaseEntityAudit
    {
        public int ID { get; set; }
        public string Description
        {
            get; set;
        }
    }

    public class VersionDto
    {
        public int Id { get; set; }
        public string VersionId { get; set; }
        public string Description
        {
            get; set;
        }
    }

    public record Value(int id, string Name);
}
