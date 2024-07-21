using System;
using System.Collections.Generic;

namespace DigitalApp2.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string UserProfile { get; set; } = null!;

    public bool UserStatus { get; set; }
}
