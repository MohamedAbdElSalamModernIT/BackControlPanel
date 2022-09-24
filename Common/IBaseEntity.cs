using Common.Interfaces;
using System;
using Common.Attributes;

namespace Common {
  public class BaseEntityAudit: BaseEntity, IAudit {
    [NotGenerated] public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
    [NotGenerated] public string CreatedBy { get; set; }
    [NotGenerated] public DateTime? UpdatedDate { get; set; }
    [NotGenerated] public string UpdatedBy { get; set; }
  }

  public class BaseEntity : IBaseEntity {
  }

  public interface IBaseEntity { }
}