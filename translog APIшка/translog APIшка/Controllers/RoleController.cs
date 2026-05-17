using Microsoft.AspNetCore.Mvc;
using translog_APIшка.Model;

namespace translog_APIшка.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    [HttpGet("getRole")]
    public IActionResult GetRole()
    {
        var db = new TransLogCourseContext();
        var roles = db.Roles.ToList();
        return Ok(roles);
    }
}