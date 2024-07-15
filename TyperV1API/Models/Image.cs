﻿using System;
using System.Collections.Generic;

namespace TyperV1API.Models;

public partial class Image
{
    public int ImageId { get; set; }

    public string ImagePath { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
