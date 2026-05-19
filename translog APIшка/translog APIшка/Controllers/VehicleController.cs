using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using translog_APIшка.Model;

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
        var db = new TransLogCourseContext();
        var vehicle = db.Vehicles
            .Include(v => v.Drivers).ThenInclude(v => v.User)
            .Include(v => v.VehicleType)
            .ToList();
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
        var db = new TransLogCourseContext();
        var newVehicle = new Vehicle
        {
            LicensePlate = model.LicensePlate,
            Brand = model.Brand,
            Model = model.Model,
            PayloadKg = model.PayloadKg,
            VolumeM3 = model.VolumeM3,
            VehicleId = model.VehicleId,
        };
        db.Vehicles.Add(newVehicle);
        db.SaveChanges();
        if (model.UserId != null)
        {
            var driver = db.Drivers.FirstOrDefault(d => d.UserId == model.UserId);
            if (driver != null)
            {
                driver.VehicleId = newVehicle.VehicleId;
            }
            else
            {
                db.Drivers.Add(new Driver
                {
                    UserId = model.UserId.Value,
                    VehicleId = newVehicle.VehicleId,
                    Working = "Активен"
                });
            }
            db.SaveChanges();
        }
        return Ok(new
        {
            message = "Машина добавлена!",
            newVehicle
        });
    }

    [HttpPut("UpdateTransport")]
    public IActionResult UpdateTransport(int vehicleId, VehicleModel model)
    {
        var db = new TransLogCourseContext();
        var vehicle = db.Vehicles
            .Include(v => v.Drivers)
            .FirstOrDefault(v => v.VehicleId == vehicleId);
    
        if (vehicle == null)
            return NotFound("Такой машины нету");

        vehicle.LicensePlate = model.LicensePlate;
        vehicle.Brand = model.Brand;
        vehicle.Model = model.Model;
        vehicle.PayloadKg = model.PayloadKg;
        vehicle.VolumeM3 = model.VolumeM3;
        vehicle.VehicleTypeId = model.VehicleTypeId;

        if (model.UserId != null)
        {
            var driver = db.Drivers.FirstOrDefault(d => d.UserId == model.UserId);
            if (driver != null)
                driver.VehicleId = vehicleId;
            else
                db.Drivers.Add(new Driver { UserId = model.UserId.Value, VehicleId = vehicleId, Working = "Активен" });
        }

        db.SaveChanges();
        return Ok(new { message = "Машина обновлена!" });
    }
}

public class VehicleModel
{
    public int VehicleId { get; set; }

    public string? LicensePlate { get; set; }

    public string? Brand { get; set; }

    public string? Model { get; set; }

    public decimal? PayloadKg { get; set; }

    public decimal? VolumeM3 { get; set; }
    public int? VehicleTypeId { get; set; }

    public int? UserId { get; set; }
}