using System;
using Domain.Entities.Auth;

namespace Domain.Entities.Notification {
  public class UserNotification {

    public int Id { get; set; }
    public int NotficiationId { get; set; }
    public string UserId { get; set; }
    public bool Read { get; set; } = false;
    //[ForeignKey("NotficiationId")]
    public Notification Notification { get; set; }
    //[ForeignKey("UserId")]
    public User User { get; set; }
    public DateTime? ReadDate { get; set; }
  }
}