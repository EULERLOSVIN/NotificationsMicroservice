using System;
using System.Collections.Generic;

namespace NotificationsMicroservice.Entities;

public partial class NotificationAttempt
{
    public int IdAttempt { get; set; }

    public int IdNotification { get; set; }

    public string Status { get; set; } = null!;

    public string? ErrorMessage { get; set; }

    public DateTime? AttemptDate { get; set; }

    public virtual NotificationLog IdNotificationNavigation { get; set; } = null!;
}
