
using Common;
using Common.Infrastructures;
using Mapster;

namespace Application.Baladya.Dto
{
    public class BaladyaDto : BaseEntityAudit,  IRegister
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Amana { get; set; }
        public int AmanaId { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Benaa.Baladia, BaladyaDto>()
                .Map(dest => dest.Amana, src => src.Amana != null ? src.Amana.Name : "");
        }
    }
}
