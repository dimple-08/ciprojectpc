using System;
using System.Collections.Generic;

namespace CIProjectweb.Entities.DataModels;

public partial class Notification
{
    public long NotificationId { get; set; }

    public long? UserId { get; set; }

    public string? NotificationText { get; set; }
}
