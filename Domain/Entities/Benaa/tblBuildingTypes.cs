using Common;

namespace Domain.Entities.Benaa
{
    public class BuildingType : BaseEntityAudit
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ActivityID { get; set; }
        public int? ExternalCode { get; set; }
    }
}
