using Domain.Enums;

namespace Application.Drawings.Dto
{
    public class DrwaingLogDto
    {
        public string Id { get; set; }
        public int SuccessNo { get; set; }
        public int FailNo { get; set; }
        public int OtherNo { get; set; }
        public ConditionStatus Result { get; set; }
        public string DrwaingId { get; set; }
    }
}
