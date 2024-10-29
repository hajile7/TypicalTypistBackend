using System;
using System.Collections.Generic;

namespace TyperV1API.Models;

public partial class Word
{
    public int WordId { get; set; }

    public string Word1 { get; set; } = null!;

    public string? StartsWith { get; set; }

    public virtual ICollection<Bigraph> Bigraphs { get; set; } = new List<Bigraph>();
}
