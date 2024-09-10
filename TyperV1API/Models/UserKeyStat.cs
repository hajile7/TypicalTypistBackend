using System;
using System.Collections.Generic;

namespace TyperV1API.Models;

public partial class UserKeyStat
{
    public int KeyStatId { get; set; }

    public int? UserId { get; set; }

    public string Key { get; set; } = null!;

    public int? TotalTyped { get; set; }

    public int? CorrectTyped { get; set; }

    public int? IncorrectTyped { get; set; }

    public decimal? KeyAccuracy { get; set; }

    public virtual User? User { get; set; }
}
