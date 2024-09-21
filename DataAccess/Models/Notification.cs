using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Notification
{
    internal object Users;

    public int NotificationId { get; set; }

    public int? UserId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime? SentDate { get; set; }

    public int? NotificationTypeId { get; set; }

    public virtual NotificationType? NotificationType { get; set; }

    public virtual User? User { get; set; }
}
