using System;
using System.Collections.Generic;

namespace TyperV1API.Models;

public partial class UserTypingTest
{
    public int TestId { get; set; }

    public int UserId { get; set; }

    public DateTime TestDate { get; set; }

    public string? Mode { get; set; }

    public decimal? Wpm { get; set; }

    public decimal? Accuracy { get; set; }
}
