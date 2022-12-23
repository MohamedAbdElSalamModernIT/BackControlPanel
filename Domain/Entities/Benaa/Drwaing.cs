using Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Benaa
{
    public class Drawing : BaseEntityAudit
    {
        public string Id { get; set; }

        public FileType FileType { get; set; }
        public DrawingType DrawingType { get; set; }
        //public string FileLink { get; set; }
        public DrawingStatus Status { get; set; } = DrawingStatus.Pending;

        public string CustomerName { get; set; }

        public string ClientId { get; set; }
        public Client Client { get; set; }

        public int BaladiaId { get; set; }
        public Baladia Baladia { get; set; }

        public int BuildingTypeId { get; set; }
        public BuildingType BuildingType { get; set; }

        public HashSet<DrawingLog> Logs { get; set; }
    }
}
