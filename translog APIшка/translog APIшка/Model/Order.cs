using System;
using System.Collections.Generic;

namespace translog_APIшка.Model;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime? ReceivedAt { get; set; }

    public int UserId { get; set; }

    public int? VehicleId { get; set; }

    public string Status { get; set; } = null!;

    public decimal? Weight { get; set; }

    public decimal? VolumeM3 { get; set; }

    public string? DeparturePoint { get; set; }

    public string? ArrivalPoint { get; set; }

    public DateOnly? DepartureTime { get; set; }

    public DateOnly? ArrivalTime { get; set; }

    public string? Description { get; set; }

    public int? DistanceKm { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Vehicle? Vehicle { get; set; }
}
