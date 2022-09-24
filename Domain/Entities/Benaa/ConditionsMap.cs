using Common;
using Common.Interfaces;
using System;

namespace Domain.Entities.Benaa
{
    //public class ConditionsMap : IAudit
    public class ConditionsMap : BaseEntityAudit
    {
        public int AlBaladiaID { get; set; }
        public Baladia Baladia { get; set; }
        public int BuildingTypeID { get; set; }
        public BuildingType BuildingType { get; set; }
        public double ConditionID { get; set; }
        public Condition Condition { get; set; }
        public string ParametersValues { get; set; }
    }
}
