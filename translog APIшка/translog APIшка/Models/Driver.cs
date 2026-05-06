using System;
using System.Collections.Generic;

namespace translog_APIшка.Models;

public partial class Driver
{
    public int DriverId { get; set; }

    public string FullName { get; set; } = null!;

    public string? LicensePlate { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
