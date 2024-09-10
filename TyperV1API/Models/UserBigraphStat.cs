using System;
using System.Collections.Generic;

namespace TyperV1API.Models;

public partial class UserBigraphStat
{
    public int BigraphStatId { get; set; }

    public int? UserId { get; set; }

    public string StartingKey { get; set; } = null!;

    public string Bigraph { get; set; } = null!;

    public int? TotalTyped { get; set; }

    public int? CorrectTyped { get; set; }

    public int? IncorrectTyped { get; set; }

    public decimal? BigraphAccuracy { get; set; }

    public virtual User? User { get; set; }
}
