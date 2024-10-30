using System;
using System.Collections.Generic;

namespace TypicalTypistAPI.Models;

public partial class UserStat
{
    public int StatId { get; set; }

    public int? UserId { get; set; }

    public int? WordId { get; set; }

    public int? BigraphId { get; set; }

    public string? InitialLetter { get; set; }

    public int? WordLength { get; set; }

    public int? Correct { get; set; }

    public int? Incorrect { get; set; }

    public int? CharCorrect { get; set; }

    public int? CharIncorrect { get; set; }

    public int? Wpm { get; set; }

    public int? Cpm { get; set; }

    public decimal? Accuracy { get; set; }

    public virtual Bigraph? Bigraph { get; set; }

    public virtual User? User { get; set; }

    public virtual Word? Word { get; set; }
}
