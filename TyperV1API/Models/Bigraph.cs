using System;
using System.Collections.Generic;

namespace TyperV1API.Models;

public partial class Bigraph
{
    public int BigraphId { get; set; }

    public string Bigraph1 { get; set; } = null!;

    public int? WordId { get; set; }

    public virtual ICollection<UserStat> UserStats { get; set; } = new List<UserStat>();

    public virtual Word? Word { get; set; }
}
