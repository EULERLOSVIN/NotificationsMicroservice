using System;
using System.Collections.Generic;

namespace NotificationsMicroservice.Entities;

public partial class NotificationAttachment
{
    public int IdAttachment { get; set; }

    public int IdNotification { get; set; }

    public string FileName { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public string FileUrl { get; set; } = null!;

    public virtual NotificationLog IdNotificationNavigation { get; set; } = null!;
}
