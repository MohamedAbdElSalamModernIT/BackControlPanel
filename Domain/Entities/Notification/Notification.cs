using System;
using System.Collections.Generic;
using Common.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Domain.Entities.Notification {
  public class Notification : IAudit {
    public Notification() {
      Users = new HashSet<UserNotification>();
      CreatedDate = DateTime.UtcNow;
    }

    public int Id { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public ICollection<UserNotification> Users { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string UpdatedBy { get; set; }
  }
}