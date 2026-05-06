using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using translog_APIшка.Models;

namespace translog_APIшка.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    [HttpGet("GetTransport")]
    public IActionResult GetTransport()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var db = new TranslogContext();
        var vehicle = db.Vehicles.ToList();
        if (vehicle.Count == 0)
        {
            return Unauthorized("Машин нету");
        }
        else
        {
            return Ok(vehicle);
        }
    }

    [HttpPost("AddTransport")]
    public IActionResult AddTransport(VehicleModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var db = new TranslogContext();
        var newVehicle = new Vehicle
        {
            LicensePlate = model.LicensePlate,
            Brand = model.Brand,
            Type = model.Type,
            LoadCapacity = model.LoadCapacity,
            BaseRatePerKm = model.BaseRatePerKm,
            DriverId = model.DriverId
        };
        db.Vehicles.Add(newVehicle);
        db.SaveChanges();
        return Ok(new
        {
            message = "Машина добавлена!",
            newVehicle
        });
    }

    [HttpPut("UpdateTransport")]
    public IActionResult UpdateTransport(int vehicleId, VehicleModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var db = new TranslogContext();
        var vehicle = db.Vehicles.FromSqlRaw($"select * from vehicles where vehicle_id = '{vehicleId}'").ToList();
        if (vehicle.Count == 0)
        {
            return Unauthorized("Такой машины нету");
        }
        else
        {
            foreach (var v in vehicle)
            {
            v.LicensePlate = model.LicensePlate;
            v.Brand = model.Brand;
            v.Type = model.Type;
            v.LoadCapacity = model.LoadCapacity;
            v.BaseRatePerKm = model.BaseRatePerKm;
            v.DriverId = model.DriverId;
            }
            db.SaveChanges();
            return Ok(new {message = "Машина обновлена!",  vehicle});
        }
    }
}

public class VehicleModel
{

    public string LicensePlate { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public string Type { get; set; } = null!;

    public decimal LoadCapacity { get; set; }

    public decimal BaseRatePerKm { get; set; }

    public int? DriverId { get; set; }

}