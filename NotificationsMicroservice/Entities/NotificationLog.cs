using System;
using System.Collections.Generic;

namespace NotificationsMicroservice.Entities;

public partial class NotificationLog
{
    public int IdNotification { get; set; }

    public int IdAccountUser { get; set; }

    public string RecipientContact { get; set; } = null!;

    public string Channel { get; set; } = null!;

    public string? Subject { get; set; }

    public string BodyContent { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<NotificationAttachment> NotificationAttachments { get; set; } = new List<NotificationAttachment>();

    public virtual ICollection<NotificationAttempt> NotificationAttempts { get; set; } = new List<NotificationAttempt>();
}
