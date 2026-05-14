using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using translog_APIшка.Model;
 
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
          var db = new TransLogCourseContext();
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
          var db = new TransLogCourseContext();
              var newDriver = new Driver
              {
                  UserId = model.UserId,
                  DriverId =  model.DriverId,
                  LicenseCategories = model.LicenseCategories,
                  VehicleId = model.VehicleId
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
      public int DriverId { get; set; }

      public int UserId { get; set; }

      public string? LicenseCategories { get; set; }

      public int? VehicleId { get; set; }
     
  }