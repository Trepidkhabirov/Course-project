using Microsoft.AspNetCore.Mvc;
using translog_APIшка.Model;

namespace translog_APIшка.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehicleTypeController : ControllerBase
{
    [HttpGet("GetTypes")]
    public IActionResult GetTypes()
    {
        var db = new TransLogCourseContext();
        var types = db.VehicleTypes.ToList();
        return Ok(types);
    }
    [HttpPost("AddType")]
    public IActionResult AddType([FromBody] VehicleTypeModel model)
    {
        var db = new TransLogCourseContext();
        var newType = new VehicleType
        {
            Name = model.Name,
            Description = model.Description,
            PricePerKm = model.PricePerKm
        };
        db.VehicleTypes.Add(newType);
        db.SaveChanges();
        return Ok(newType);
    }

    [HttpPut("UpdateType")]
    public IActionResult UpdateType([FromBody] VehicleTypeModel model)
    {
        var db = new TransLogCourseContext();
        var type = db.VehicleTypes.FirstOrDefault(t => t.TypeId == model.TypeId);
        if (type == null) return NotFound();
        type.PricePerKm = model.PricePerKm;
        db.SaveChanges();
        return Ok(type);
    }
}
public class VehicleTypeModel
{
    public int TypeId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal PricePerKm { get; set; }
}
