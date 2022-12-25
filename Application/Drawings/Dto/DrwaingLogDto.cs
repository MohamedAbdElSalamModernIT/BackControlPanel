using Domain.Enums;
using System;

namespace Application.Drawings.Dto
{
    public class DrwaingLogDto
    {
        public string Id { get; set; }
        public int SuccessNo { get; set; }
        public int FailNo { get; set; }
        public int OtherNo { get; set; }
        public ConditionStatus Result { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
