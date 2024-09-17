using System;
using System.Collections.Generic;

namespace WebApi_ModelsCons_.Models;

public partial class NotificationType
{
    public int NotificationTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
