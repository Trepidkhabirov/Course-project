using System;
using System.Collections.Generic;

namespace translog_APIшка.Model;

public partial class VehicleType
{
    public int TypeId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? PricePerKm { get; set; }

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
