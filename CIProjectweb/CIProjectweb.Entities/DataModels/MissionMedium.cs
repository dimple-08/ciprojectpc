﻿using System;
using System.Collections.Generic;

namespace CIProjectweb.Entities.DataModels;

public partial class MissionMedium
{
    public long MissionMediaId { get; set; }

    public long MissionId { get; set; }

    public string MediaName { get; set; } = null!;

    public string MediaType { get; set; } = null!;

    public string? MediaPath { get; set; }

    public bool? Default { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Mission Mission { get; set; } = null!;
}
