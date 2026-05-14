using System;
using System.Collections.Generic;

namespace translog_APIшка.Model;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Numberphone { get; set; }

    public int RoleId { get; set; }

    public string? FullName { get; set; }

    public int? IsActive { get; set; }

    public virtual Driver? Driver { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;
}
