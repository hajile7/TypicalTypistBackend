﻿using System;
using System.Collections.Generic;

namespace TyperV1API.Models;

public partial class UserStat
{
    public int StatId { get; set; }

    public int? UserId { get; set; }

    public int? BigraphId { get; set; }

    public decimal? TopWpm { get; set; }

    public decimal? Wpm { get; set; }

    public decimal? Cpm { get; set; }

    public decimal? TopAccuracy { get; set; }

    public decimal? Accuracy { get; set; }

    public virtual Bigraph? Bigraph { get; set; }

    public virtual User? User { get; set; }
}
