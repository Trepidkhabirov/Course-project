using System;
using System.Collections.Generic;

namespace translog_APIшка.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime ReceivedAt { get; set; }

    public string Status { get; set; } = null!;

    public decimal Weight { get; set; }

    public decimal VolumeM3 { get; set; }

    public string Phone { get; set; } = null!;

    public string DeparturePoint { get; set; } = null!;

    public string ArrivalPoint { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Description { get; set; }
    
    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
