using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using translog_APIшка.Models;

namespace translog_APIшка.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriverController : ControllerBase
{
    [HttpGet("GetDrivers")]
    public IActionResult GetDrivers()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var db = new TranslogContext();
        var drivers = db.Drivers.ToList();
        if (drivers.Count() == 0)
            return Unauthorized("Водителей нету");
        else
        {
            return Ok(drivers);           
        }
    }

    [HttpPost("AddDriver")]
    public IActionResult AddDriver(DriverModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var db = new TranslogContext();
            var newDriver = new Driver
            {
                FullName = model.FullName,
                LicensePlate = model.LicensePlate,
                UserId = model.UserId
            };
            db.Drivers.Add(newDriver);
            db.SaveChanges();
            return Ok(new
            {
                message = "Водитель добавлен!",
                newDriver
            });
    }
}

public class DriverModel
{
    public string FullName { get; set; } = null!;

    public string? LicensePlate { get; set; }

    public int? UserId { get; set; }
   
}