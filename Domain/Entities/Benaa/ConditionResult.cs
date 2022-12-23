using Common;
using Domain.Enums;

namespace Domain.Entities.Benaa
{
    public class ConditionResult : BaseEntityAudit
    {
        public string Id { get; set; }
        public ConditionStatus Status { get; set; }
        public string CurrentCondition { get; set; }

        public double ConditionId { get; set; }
        public Condition Condition { get; set; }

        public string LogId { get; set; }
        public DrawingLog Log { get; set; }
    }
}
