using System;
using System.Collections.Generic;

namespace TyperV1API.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? ImageId { get; set; }

    public int? Wpm { get; set; }

    public bool? Active { get; set; }

    public virtual Image? Image { get; set; }
}
