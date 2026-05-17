using System;
using System.Collections.Generic;

namespace translog_APIшка.Model;

public partial class Vehicle
{
    public int VehicleId { get; set; }

    public string? LicensePlate { get; set; }

    public string? Brand { get; set; }

    public string? Model { get; set; }

    public decimal? PayloadKg { get; set; }

    public decimal? VolumeM3 { get; set; }

    public int? VehicleTypeId { get; set; }

    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual VehicleType? VehicleType { get; set; }
}
