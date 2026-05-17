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
        var vehicle = db.Vehicles.Include(v => v.Drivers).ThenInclude(v => v.User).ToList();
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
        var db = new TransLogCourseContext();
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
                v.Model = model.Model;
                v.PayloadKg = model.PayloadKg;
                v.VolumeM3 = model.VolumeM3;
                v.VehicleId = model.VehicleId;
            }
            db.SaveChanges();
            return Ok(new {message = "Машина обновлена!",  vehicle});
        }
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

}