using Common;
using Domain.Enums;
using System.Collections.Generic;

namespace Domain.Entities.Benaa
{
    public class DrawingLog : BaseEntityAudit
    {
        public string Id { get; set; }
        public int SuccessNo { get; set; }
        public int FailNo { get; set; }
        public int OtherNo { get; set; }
        public ConditionStatus Result { get; set; }

        public string DrwaingId { get; set; }
        public Drawing Drwaing { get; set; }

        public HashSet<ConditionResult> Results { get; set; }
    }
}
