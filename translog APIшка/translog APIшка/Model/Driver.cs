using System;
using System.Collections.Generic;

namespace translog_APIшка.Model;

public partial class Driver
{
    public int DriverId { get; set; }

    public int UserId { get; set; }

    public string? LicenseCategories { get; set; }

    public int? VehicleId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Vehicle? Vehicle { get; set; }
}
