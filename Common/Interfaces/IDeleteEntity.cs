using System;

namespace Common.Interfaces
{
  public interface IDeleteEntity
  {
    bool IsDeleted { get; set; }
    string DeletedBy { get; set; }
    DateTime? DeletedDate { get; set; }
  }
}