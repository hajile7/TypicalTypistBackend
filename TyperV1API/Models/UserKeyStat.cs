using System;
using System.Collections.Generic;

namespace TyperV1API.Models;

public partial class UserKeyStat
{
    public int KeyStatId { get; set; }

    public int? UserId { get; set; }

    public string Key { get; set; } = null!;

    public int? TotalTyped { get; set; }

    public int? Accuracy { get; set; }

    public int? Speed { get; set; }

    public virtual User? User { get; set; }
}
