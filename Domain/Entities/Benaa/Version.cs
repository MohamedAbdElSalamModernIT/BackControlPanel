using System;

namespace Domain.Entities.Benaa
{
    public class Version 
    {
        public int Id { get; set; }
        public string VersionId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

    }


}
