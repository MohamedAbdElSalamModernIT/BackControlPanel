using System;
using System.Text;

namespace Common.Interfaces {
  public interface IAudit {
    DateTime? CreatedDate { get; set; }
    string CreatedBy { get; set; }
    DateTime? UpdatedDate { get; set; }
    string UpdatedBy { get; set; }
  }
}
