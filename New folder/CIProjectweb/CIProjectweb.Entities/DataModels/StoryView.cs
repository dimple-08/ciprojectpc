using System;
using System.Collections.Generic;

namespace CIProjectweb.Entities.DataModels;

public partial class StoryView
{
    public long? UserId { get; set; }

    public long? StoryId { get; set; }

    public long StoryviewId { get; set; }
}
