using Common;
using System.Collections.Generic;

namespace Domain.Entities.Benaa
{
    public class Condition: BaseEntityAudit
    {
        public double ID { get; set; }
        public string Description { get; set; }
        public string Rules { get; set; }
        public string Facts { get; set; }
        public int? PlaceID { get; set; } 

        public int VersionId { get; set; }
        public Version Version { get; set; }

        public Place Place { get; set; }

        public string CategoryId { get; set; }
        public Category Category { get; set; }

        public double Code { get; set; }
        public string RulesNew { get; set; }

        public HashSet<ConditionsMap> ConditionsMaps { get; set; }
    }
}
