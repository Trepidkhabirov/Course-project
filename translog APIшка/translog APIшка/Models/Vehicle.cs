using System;
using System.Collections.Generic;

namespace translog_APIшка.Models;

public partial class Vehicle
{
    public int VehicleId { get; set; }

    public string LicensePlate { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public string Type { get; set; } = null!;

    public decimal LoadCapacity { get; set; }

    public decimal BaseRatePerKm { get; set; }

    public int? DriverId { get; set; }

    public virtual Driver? Driver { get; set; }

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
