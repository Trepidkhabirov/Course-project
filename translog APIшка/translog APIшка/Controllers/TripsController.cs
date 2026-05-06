using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using translog_APIшка.Models;

namespace translog_APIшка.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    [HttpGet("GetTrips")]
    public IActionResult GetTrips()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var db = new TranslogContext();
        var trips = db.Trips.FromSqlRaw($"select * from trips").ToList();
        if (trips.Count() == 0)
        {
            return Unauthorized("Рейсов нету");
        }
        else
        {
            return Ok(trips);
        }
    }

    [HttpPost("AddTrips")]
    public IActionResult AddTrips(TripsModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var db = new TranslogContext();
        var newTrips = new Trip
        {
            OrderId = model.OrderId,
            VehicleId = model.vehicleId,
            DriverId = model.DriverId,
            DeparturePoint = model.departurePoint,
            ArrivalPoint = model.arrivalPoint,
            DepartureTime = model.departureTime,
            ArrivalTime = model.arrivalTime,
            DistanceKm = model.distanceKm
        };
        db.Trips.Add(newTrips);
        db.SaveChanges();
        return Ok(new
        {
            message = "Рейс добавлен!",
            OrderId = newTrips.OrderId,
            VehicleId = newTrips.VehicleId,
            DriverId = newTrips.DriverId,
            DeparturePoint = newTrips.DeparturePoint,
            ArrivalPoint = newTrips.ArrivalPoint,
            DepartureTime = newTrips.DepartureTime,
            ArrivalTime = newTrips.ArrivalTime,
            DistanceKm = newTrips.DistanceKm
        });
    }
}

public class TripsModel
{
    public int OrderId { get; set; }
    public  int vehicleId { get; set; }
    public int DriverId { get; set; }
    public string departurePoint  { get; set; }
    public string arrivalPoint { get; set; }
    public DateTime departureTime { get; set; }
    public DateTime arrivalTime { get; set; }
    public decimal distanceKm { get; set; }
}