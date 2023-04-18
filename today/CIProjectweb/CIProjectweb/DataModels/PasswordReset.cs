using System;
using System.Collections.Generic;

namespace CIProjectweb.DataModels;

public partial class PasswordReset
{
    public string Email { get; set; } = null!;

    public string Token { get; set; } = null!;

    public DateTime CreratedAt { get; set; }
}
