
using Common;
using Common.Infrastructures;
using Mapster;

namespace Application.Amana.Dto
{
    public class AmanaDto : BaseEntityAudit
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
