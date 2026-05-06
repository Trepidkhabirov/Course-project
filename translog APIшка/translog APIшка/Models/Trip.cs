using System;
using System.Collections.Generic;

namespace translog_APIшка.Models;

public partial class Trip
{
    public int TripId { get; set; }

    public int? OrderId { get; set; }

    public int? VehicleId { get; set; }

    public int? DriverId { get; set; }

    public string DeparturePoint { get; set; } = null!;

    public string ArrivalPoint { get; set; } = null!;

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public decimal? DistanceKm { get; set; }

    public virtual Vehicle? Vehicle { get; set; }
}
