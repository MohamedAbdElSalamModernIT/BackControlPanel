using Common;
using Common.Interfaces;
using System;

namespace Domain.Entities.Benaa
{
    public class Area
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
   

    public class Amana : BaseEntityAudit
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Area Area { get; set; }
        public int AreaId { get; set; }
    }

    public class Baladia : BaseEntityAudit
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Amana Amana { get; set; }
        public int AmanaId { get; set; }
    }

    public class Information : BaseEntityAudit
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Rules { get; set; }
        public string Facts { get; set; }
        public double Code { get; set; }
        public string RulesNew { get; set; }
        public int? TypeId { get; set; }
    }
}
