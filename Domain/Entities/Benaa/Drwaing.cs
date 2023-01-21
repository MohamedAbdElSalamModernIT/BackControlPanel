using Common;
using Common.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Benaa
{
    public class Drawing : BaseEntityAudit, IDeleteEntity
    {
        public string Id { get; set; }
        public FileType FileType { get; set; }
        public DrawingType DrawingType { get; set; }
        //public string FileLink { get; set; }
        public DrawingStatus Status { get; set; } = DrawingStatus.Pending;

        public string CustomerName { get; set; }

        public OfficeDrawingStatus OfficeStatus { get; set; } = OfficeDrawingStatus.NotAssigned;
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string Comments { get; set; }

        public string OfficeId { get; set; }
        public Office Office { get; set; }

        public string EngineerId { get; set; }
        public AppUser Engineer { get; set; }

        public int BaladiaId { get; set; }
        public Baladia Baladia { get; set; }

        public int RequestNo { get; set; }
        public int BuildingTypeId { get; set; }

        public BuildingType BuildingType { get; set; }

        public byte[] File { get; set; }
        public string Extension { get; set; }
        public HashSet<DrawingLog> Logs { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
